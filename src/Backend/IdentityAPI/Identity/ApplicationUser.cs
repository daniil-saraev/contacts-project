using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Identity
{
    //Add any custom properties
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }
    }
}
