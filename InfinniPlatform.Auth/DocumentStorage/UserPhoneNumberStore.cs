using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserPhoneNumberStoreExtended<TUser> where TUser : AppUser
    {
        /// <inheritdoc />
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken token)
        {
            user.PhoneNumber = phoneNumber;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        /// <inheritdoc />
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <inheritdoc />
        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            user.PhoneNumberConfirmed = confirmed;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<TUser> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            return FindUserInCache(() => (TUser) _userCache.FindUserByPhoneNumber(phoneNumber),
                                   async () => await _users.Value.Find(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync());
        }
    }
}