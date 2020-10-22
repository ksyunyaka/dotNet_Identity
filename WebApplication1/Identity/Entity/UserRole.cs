using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication1.Identity.Entity
{
    public class UserRole : IdentityRole
    {
        public UserRole()
        {
            this.Claims = new List<Claim>();
        }
       
        public IList<Claim> Claims { get; set; }
    }
}
