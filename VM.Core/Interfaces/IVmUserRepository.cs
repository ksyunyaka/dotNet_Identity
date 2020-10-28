using System;
using System.Collections.Generic;
using System.Text;
using VM.Core.Entities;

namespace VM.Core.Interfaces
{
    public interface IVmUserRepository
    {
        public VmUser GetUserByUserName(string userName);

        public VmUser CreateUser(VmUser user);

        public VmUserRole GetRole(string roleName);
    }
}
