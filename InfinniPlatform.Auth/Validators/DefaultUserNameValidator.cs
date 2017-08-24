using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Properties;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Validators
{
    /// <summary>
    /// Проверяет корректность имени пользователя.
    /// </summary>
    public class DefaultUserNameValidator<TUser> : IUserValidator<TUser> where TUser : AppUser
    {
        private readonly IUserStore<TUser> _userStore;

        public DefaultUserNameValidator(IUserStore<TUser> userStore)
        {
            _userStore = userStore;
        }


        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();
            var userName = user.UserName;

            // Имя пользователя является обязательным
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
                // Проверка уникальности
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