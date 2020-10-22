using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Identity.Dao;
using WebApplication1.Identity.Entity;

namespace WebApplication1.Identity.Stores
{
    public class AppUserStore : IUserStore<AppUser>,
                         IUserEmailStore<AppUser>,
                         IUserRoleStore<AppUser>,
                         IUserPasswordStore<AppUser>
    {
        private SqlDao daoService;

        public AppUserStore(SqlDao service)
        {
            daoService = service;
        }

        public Task AddToRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            UserRole role = daoService.getRole(roleName);
            if(role == null) throw new ArgumentException(nameof(roleName), $"The role with the given name {roleName} does not exist");

            user.Roles.Add(role);
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            int res = await daoService.CreateUser(user);
            if(res > 0)
            {
                return IdentityResult.Success;
            }else
            {
                return IdentityResult.Failed(new IdentityError() { Description = "failed to create user" });
            }
        }

        public Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("DeleteAsync");
        }

        public void Dispose()
        {
            Console.WriteLine("User dispose invoked");
        }

        public Task<AppUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("fBEmail");
        }

        public Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("FindId");
        }

        public Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("fName");
        }

        public Task<string> GetEmailAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("gEmail");
        }

        public Task<bool> GetEmailConfirmedAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("gEmConfir");
        }

        public Task<string> GetNormalizedEmailAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("gNoEmail");
        }

        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("GNUN");
        }

        public Task<string> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("GPH");
        }

        public Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("GRA");
        }

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("GUIA");
        }

        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("GUN");
        }

        public Task<IList<AppUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("GUR");
        }

        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("HAsPAs");
        }

        public Task<bool> IsInRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("IsInRole");
        }

        public Task RemoveFromRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("RemoveRoleAsync");
        }

        public Task SetEmailAsync(AppUser user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("setEmail");
        }

        public Task SetEmailConfirmedAsync(AppUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("setEConfi");
        }

        public Task SetNormalizedEmailAsync(AppUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("setNorEmai");
        }

        public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("SetNUseName");
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("SetPHas");
        }

        public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("SetUseN");
        }

        public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("UpdateAs");
        }
    }
}
