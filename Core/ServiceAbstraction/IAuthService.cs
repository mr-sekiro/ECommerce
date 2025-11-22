using Shared.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(AuthRegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(AuthLoginDto dto);
    }
}
