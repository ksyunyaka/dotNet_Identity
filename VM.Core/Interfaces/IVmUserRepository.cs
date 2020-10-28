using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VM.Core.Entities;

namespace VM.Core.Interfaces
{
    public interface IVmUserRepository
    {
        public Task<VmUser> GetUserByUserName(string userName);

        public Task<VmUser> CreateUserAsync(VmUser user);

        public VmUserRole GetRole(string roleName);
    }
}
