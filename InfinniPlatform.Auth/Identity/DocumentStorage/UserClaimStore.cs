using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public partial class UserStore<TUser> : IUserClaimStore<TUser> where TUser : AppUser
    {
        public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                var claims = new List<Claim>();

                                if (user.Claims != null)
                                {
                                    claims.AddRange(user.Claims.Select(c => c.ToSecurityClaim()));
                                }

                                return (IList<Claim>) claims;
                            }, token);
        }

        public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            foreach (var claim in claims)
            {
                user.AddClaim(claim);
            }

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            foreach (var claim in claims)
            {
                user.RemoveClaim(claim);
            }

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken token)
        {
            user.ReplaceClaim(claim, newClaim);

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken token)
        {
            return await Users.Value.Find(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value)).ToListAsync();
        }
    }
}