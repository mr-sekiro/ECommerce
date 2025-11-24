using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtClaims = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(AuthRegisterDto dto)
        {
            // Check if user exists
            if (await _userManager.FindByEmailAsync(dto.Email) != null)
                throw new UserAlreadyExistsException(dto.Email);

            var user = new ApplicationUser
            {
                DisplayName = dto.DisplayName,
                UserName = dto.UserName,
                Email = dto.Email,
                EmailConfirmed = true
            };

            // Create user
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new IdentityOperationException(result.Errors);

            // Reload user to ensure ID is populated
            var createdUser = await _userManager.FindByEmailAsync(dto.Email)
                              ?? throw new Exception("Failed to retrieve the newly created user.");

            // Assign default role
            if (!await _roleManager.RoleExistsAsync("Customer"))
                await _roleManager.CreateAsync(new IdentityRole("Customer"));

            await _userManager.AddToRoleAsync(createdUser, "Customer");

            return await GenerateJwtAsync(createdUser);
        }

        public async Task<AuthResponseDto> LoginAsync(AuthLoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new UserNotFoundException(dto.Email);

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedException();

            return await GenerateJwtAsync(user);
        }

        private async Task<AuthResponseDto> GenerateJwtAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtClaims.Sub, user.Id),
                new Claim(JwtClaims.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public async Task<AddressDto?> GetUserAddressAsync(string email)
        {
            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) throw new UserNotFoundException(email);

            if (user.Address == null) return null;

            return new AddressDto
            {
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName,
                Street = user.Address.Street,
                City = user.Address.City,
                Country = user.Address.Country
            };
        }

        public async Task<AddressDto?> UpdateUserAddressAsync(string email, AddressDto dto)
        {
            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) throw new UserNotFoundException(email);

            user.Address = new Address
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Street = dto.Street,
                City = dto.City,
                Country = dto.Country
            };

            await _userManager.UpdateAsync(user);

            return dto;
        }

        public async Task<bool> CheckEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<AuthResponseDto> GetCurrentUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                       ?? throw new UserNotFoundException(email);

            return await GenerateJwtAsync(user);
        }


    }
}
