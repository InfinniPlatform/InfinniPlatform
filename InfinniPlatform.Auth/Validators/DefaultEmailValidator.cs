using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Properties;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Validators
{
    /// <summary>
    /// Проверяет корректность адреса электронной почты пользователя.
    /// </summary>
    public class DefaultEmailValidator<TUser> : IUserValidator<TUser> where TUser : AppUser
    {
        private readonly IUserStore<TUser> _userStore;
        private const string EmailRegex = "^.*?@.*?\\.*.?$";

        public DefaultEmailValidator(IUserStore<TUser> userStore)
        {
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();
            var email = user.Email;

            // Если Email задан
            if (!string.IsNullOrWhiteSpace(email))
            {
                // Проверка корректности Email
                var match = Regex.IsMatch(email, EmailRegex);

                if (!match)
                {
                    errors.Add(new IdentityError {Description = string.Format(Resources.InvalidEmail, email)});
                }

                // Проверка уникальности Email

                var store = _userStore as IUserEmailStore<TUser>;

                if (store != null)
                {
                    var owner = await store.FindByEmailAsync(email, default(CancellationToken));

                    if (owner != null && !string.Equals((string) owner._id, (string) user._id))
                    {
                        errors.Add(new IdentityError {Description = string.Format(Resources.DuplicateEmail, email)});
                    }
                }
            }

            var result = errors.Count <= 0
                             ? IdentityResult.Success
                             : IdentityResult.Failed(errors.ToArray());

            return result;
        }
    }
}