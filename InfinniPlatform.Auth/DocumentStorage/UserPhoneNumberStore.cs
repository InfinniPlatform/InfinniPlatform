using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserPhoneNumberStoreExtended<TUser> where TUser : AppUser
    {
        public async Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken token)
        {
            user.PhoneNumber = phoneNumber;

            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            user.PhoneNumberConfirmed = confirmed;

            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);
            UpdateUserInCache(user);
        }

        public Task<TUser> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            return FindUserInCache(() => (TUser) UserCache.FindUserByPhoneNumber(phoneNumber),
                                   async () => await Users.Value.Find(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync());
        }
    }
}