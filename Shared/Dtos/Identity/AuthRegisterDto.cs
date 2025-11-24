using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Identity
{
    public class AuthRegisterDto
    {
        public string DisplayName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string PhoneNnmber { get; set; } = default!;
    }
}
