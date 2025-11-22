using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class IdentityOperationException : Exception
    {
        public IEnumerable<IdentityError>? Errors { get; }

        public IdentityOperationException(IEnumerable<IdentityError>? errors)
            : base("One or more Identity operations failed.")
        {
            Errors = errors;
        }

        public override string ToString()
        {
            if (Errors == null) return base.ToString();

            return $"{Message} Errors: {string.Join(", ", Errors)}";
        }
    }
}
