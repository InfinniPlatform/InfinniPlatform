using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Properties;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Validators
{
    /// <summary>
    /// Проверяет корректность телефонного номера пользователя.
    /// </summary>
    public class DefaultPhoneNumberValidator<TUser> : IUserValidator<TUser> where TUser : AppUser
    {
        private readonly IUserPhoneNumberStoreExtended<TUser> _userStore;

        public DefaultPhoneNumberValidator(IUserPhoneNumberStoreExtended<TUser> userStore)
        {
            _userStore = userStore;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();
            var phoneNumber = user.PhoneNumber;
            
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                // Проверка уникальности номера телефона
                var owner = await _userStore.FindByPhoneNumberAsync(phoneNumber, CancellationToken.None);

                if (owner != null && !string.Equals(owner.Id, user.Id))
                {
                    errors.Add(new IdentityError {Description = string.Format(Resources.DuplicatePhoneNumber, phoneNumber)});
                }
            }

            var result = errors.Count <= 0
                             ? IdentityResult.Success
                             : IdentityResult.Failed(errors.ToArray());

            return result;
        }
    }
}