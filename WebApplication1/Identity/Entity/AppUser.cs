using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication1.Identity.Entity
{
    public class AppUser : IdentityUser
    {

        public AppUser()
        {
            this.Roles = new List<IdentityRole>();
            this.Claims = new List<Claim>();
        }


        public IList<Claim> Claims { get; set; }

        public IList<IdentityRole> Roles { get; set; }

    }

}
