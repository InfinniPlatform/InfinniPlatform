using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserClaimStore<TUser> where TUser : AppUser
    {
        /// <inheritdoc />
        public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult((IList<Claim>)user.Claims.Select(c => c.ToSecurityClaim()).ToList());
        }

        /// <inheritdoc />
        public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            foreach (var claim in claims)
            {
                user.AddClaim(claim);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            foreach (var claim in claims)
            {
                user.RemoveClaim(claim);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken token)
        {
            user.ReplaceClaim(claim, newClaim);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken token)
        {
            return await _users.Value.Find(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value)).ToListAsync();
        }
    }
}