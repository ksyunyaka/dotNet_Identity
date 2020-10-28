using System;
using System.Collections.Generic;
using System.Text;

namespace VM.Core.Entities
{
    public class VmUser
    {
        public VmUser()
        {
            UserRoles = new List<VmUserRole>();
        }

        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }

        public string Email { get; set; }
        public string NormalizedEmail { get; set; }

        public string PasswordHash { get; set; }

        public string Organization { get; set; }

        public bool IsDeleted { get; set; }

        public IList<VmUserRole> UserRoles { get; set; }
    }
}
