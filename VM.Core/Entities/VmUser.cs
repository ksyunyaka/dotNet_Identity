using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VM.Core.Entities
{
    public class VmUser
    {
        public VmUser()
        {
            UserRoles = new List<VmUserRole>();
            Id = Guid.NewGuid().ToString();
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }

        public string Email { get; set; }
        public string NormalizedEmail { get; set; }

        public string PasswordHash { get; set; }

        public string Organization { get; set; }

        public string FirsName { get; set; }
        public string LastName { get; set; }

        public bool IsDeleted { get; set; }

        public IList<VmUserRole> UserRoles { get; set; }
    }
}
