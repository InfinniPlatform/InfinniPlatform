using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Properties;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Validators
{
    /// <summary>
    /// Validates user email.
    /// </summary>
    public class DefaultUserNameValidator<TUser> : IUserValidator<TUser> where TUser : AppUser
    {
        private readonly IUserStore<TUser> _userStore;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultUserNameValidator{TUser}" />.
        /// </summary>
        /// <param name="userStore">User store.</param>
        public DefaultUserNameValidator(IUserStore<TUser> userStore)
        {
            _userStore = userStore;
        }

        /// <summary>
        /// Validates email.
        /// </summary>
        /// <param name="manager">User manager.</param>
        /// <param name="user">User information.</param>
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();
            var userName = user.UserName;

            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(new IdentityError
                           {
                               Description = Resources.UserNameCannotBeNullOrWhiteSpace
                           });
            }
            else if (!Regex.IsMatch(userName, @"^[A-Za-z0-9@_\+\-\.]+$", RegexOptions.Compiled)) // Проверка корректности имени пользователя
            {
                errors.Add(new IdentityError
                           {
                               Description = string.Format(Resources.InvalidUserName, userName)
                           });
            }
            else
            {
                var owner = await _userStore.FindByNameAsync(userName, default(CancellationToken));

                if (owner != null && !Equals(owner._id, user._id))
                {
                    errors.Add(new IdentityError
                               {
                                   Description = string.Format(Resources.DuplicateUserName, userName)
                               });
                }
            }

            var result = errors.Count <= 0
                             ? IdentityResult.Success
                             : IdentityResult.Failed(errors.ToArray());

            return result;
        }
    }
}