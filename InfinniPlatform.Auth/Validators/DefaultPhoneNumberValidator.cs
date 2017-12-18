using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Properties;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Validators
{
    /// <summary>
    /// Validates user phone number.
    /// </summary>
    public class DefaultPhoneNumberValidator<TUser> : IUserValidator<TUser> where TUser : AppUser
    {
        private readonly IUserPhoneNumberStoreExtended<TUser> _userStore;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultPhoneNumberValidator{TUser}" />.
        /// </summary>
        /// <param name="userStore">User phone number store.</param>
        public DefaultPhoneNumberValidator(IUserPhoneNumberStoreExtended<TUser> userStore)
        {
            _userStore = userStore;
        }

        /// <summary>
        /// Validates phone number.
        /// </summary>
        /// <param name="manager">User manager.</param>
        /// <param name="user">User information.</param>
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();
            var phoneNumber = user.PhoneNumber;
            
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                var owner = await _userStore.FindByPhoneNumberAsync(phoneNumber, CancellationToken.None);

                if (owner != null && !Equals(owner._id, user._id))
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