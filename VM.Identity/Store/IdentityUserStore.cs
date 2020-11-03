using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VM.Core.Entities;
using VM.Core.Interfaces;

namespace VM.Identity.Store
{
    public class IdentityUserStore : IUserStore<VmUser>,
                         IUserEmailStore<VmUser>,
                         IUserRoleStore<VmUser>,
                         IUserPasswordStore<VmUser>
    {
        private readonly IVmUserRepository userRepository;

        public IdentityUserStore(IVmUserRepository service)
        {
            userRepository = service;
        }

        public Task AddToRoleAsync(VmUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            VmUserRole role = userRepository.GetRole(roleName);
            if (role == null) throw new ArgumentException(nameof(roleName), $"The role with the given name {roleName} does not exist");

            user.UserRoles.Add(role);
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            VmUser vmUser = await userRepository.CreateUserAsync(user);
            if (vmUser != null)
            {
                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed(new IdentityError() { Description = "failed to create user" });
            }
        }

        public Task<IdentityResult> DeleteAsync(VmUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("DeleteAsync");
        }

        public void Dispose()
        {
            Console.WriteLine("User dispose invoked");
        }

        public Task<VmUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("fBEmail");
        }

        public async Task<VmUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await userRepository.GetUserByUserId(userId);
        }

        public async Task<VmUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            VmUser user = await userRepository.GetUserByUserName(normalizedUserName);
            return user;
        }

        public Task<string> GetEmailAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(true);
        }

        public Task<string> GetNormalizedEmailAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.PasswordHash);
        }

        public Task<IList<string>> GetRolesAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IList<string> userRoles = user.UserRoles.Select(r => r.Name).ToList();

            return Task.FromResult(userRoles);
        }

        public Task<string> GetUserIdAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.UserName);
        }

        public Task<IList<VmUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("GetUsersInRoleAsync");
        }

        public Task<bool> HasPasswordAsync(VmUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            String pass = user.PasswordHash;
            return Task.FromResult(pass != null);
        }

        public Task<bool> IsInRoleAsync(VmUser user, string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (normalizedRoleName == null)
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            bool isInRole = user.UserRoles.Any(r => r.Name.Equals(normalizedRoleName));

            return Task.FromResult(isInRole);
        }

        public Task RemoveFromRoleAsync(VmUser user, string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();


            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (normalizedRoleName == null)
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            VmUserRole roleToRemove = user.UserRoles.FirstOrDefault(r => r.Name == normalizedRoleName);

            if (roleToRemove != null)
            {
                user.UserRoles.Remove(roleToRemove);
            }

            return Task.CompletedTask;
        }

        public Task SetEmailAsync(VmUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(VmUser user, bool confirmed, CancellationToken cancellationToken)
        {
            Console.WriteLine("Email confirmed set");
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(VmUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(VmUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(VmUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(VmUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(VmUser user, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException("UpdateAs");
            return Task.FromResult(IdentityResult.Success); //TODO
        }
    }
}
