using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VM.Core.Entities;
using VM.Core.Interfaces;

namespace VM.Identity.Store
{
    public class IdentityRoleStore : IRoleStore<VmUserRole>
    {
        private readonly IVmUserRepository userRepository;

        public IdentityRoleStore(IVmUserRepository repo)
        {
            userRepository = repo;
        }

        public Task<IdentityResult> CreateAsync(VmUserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Create.role");
        }

        public Task<IdentityResult> DeleteAsync(VmUserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Delete.role");
        }

        public void Dispose()
        {
            Console.WriteLine("dispose.role");
        }

        public Task<VmUserRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<VmUserRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(userRepository.GetRole(normalizedRoleName));
        }

        public Task<string> GetNormalizedRoleNameAsync(VmUserRole role, CancellationToken cancellationToken)
        {

            return Task.FromResult(role.Name);
        }

        public Task<string> GetRoleIdAsync(VmUserRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task<string> GetRoleNameAsync(VmUserRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(VmUserRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.Name = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(VmUserRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName.ToUpper();
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(VmUserRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Update.Role");
        }
    }
}
