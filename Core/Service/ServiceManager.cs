using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IBasketService> _basketService;
        private readonly Lazy<IAuthService> _authService;

        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IRedisBasketRepo redisBasketRepo, UserManager<ApplicationUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IConfiguration configuration)
        {
            _productService = new Lazy<IProductService>(() =>
                new ProductService(unitOfWork, mapper));

            _basketService = new Lazy<IBasketService>(() =>
               new BasketService(redisBasketRepo, mapper));

            _authService = new Lazy<IAuthService>(() =>
               new AuthService(userManager, roleManager, configuration));
        }

        public IProductService ProductService => _productService.Value;
        public IBasketService BasketService => _basketService.Value;
        public IAuthService AuthService => _authService.Value;
    }
}
