using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.Auth.DocumentStorage
{
    public partial class UserStore<TUser> : IUserPhoneNumberStoreExtended<TUser> where TUser : AppUser
    {
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken token)
        {
            user.PhoneNumber = phoneNumber;

            return Task.CompletedTask;
        }

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken token)
        {
            user.PhoneNumberConfirmed = confirmed;

            return Task.CompletedTask;
        }

        public Task<TUser> FindByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken)
        {
            return FindUserInCache(() => (TUser) UserCache.FindUserByPhoneNumber(phoneNumber),
                                   async () => await Users.Value.Find(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync());
        }
    }
}