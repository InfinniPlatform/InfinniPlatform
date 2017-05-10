using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    public class UserClaimStore<TUser> : UserStore<TUser>, IUserClaimStore<TUser> where TUser : AppUser
    {
        public UserClaimStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache) : base(documentStorageFactory, userCache)
        {
        }

        public virtual Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken token)
        {
            return Task.Run(() => user.Claims.Select(c => c.ToSecurityClaim()) as IList<Claim>, token);
        }

        public virtual Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                foreach (var claim in claims)
                                {
                                    user.AddClaim(claim);
                                }

                                UpdateUserInCache(user);
                            }, token);
        }

        public virtual Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken token)
        {
            return Task.Run(() =>
                            {
                                foreach (var claim in claims)
                                {
                                    user.RemoveClaim(claim);
                                }

                                UpdateUserInCache(user);
                            }, token);
        }

        public virtual Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken token = default(CancellationToken))
        {
            return Task.Run(() => user.ReplaceClaim(claim, newClaim), token);
        }

        public virtual async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken token = default(CancellationToken))
        {
            return await Users.Value.Find(u => u.Claims.Any(c => c.Type == claim.Type && c.Value == claim.Value)).ToListAsync();
        }
    }
}