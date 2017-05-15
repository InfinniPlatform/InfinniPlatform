﻿using System.Collections.Generic;
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
            return Task.FromResult((IList<Claim>)user.Claims.Select(c => c.ToSecurityClaim()).ToList());
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