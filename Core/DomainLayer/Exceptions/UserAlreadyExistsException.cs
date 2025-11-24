using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class UserAlreadyExistsException(string Email): Exception($"User With Email {Email} Already Exist")
    {
    }
}
