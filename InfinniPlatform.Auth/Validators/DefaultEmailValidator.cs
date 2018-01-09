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
    /// Validates user email.
    /// </summary>
    public class DefaultEmailValidator<TUser> : IUserValidator<TUser> where TUser : AppUser
    {
        private readonly IUserStore<TUser> _userStore;
        private const string EmailRegex = "^.*?@.*?\\.*.?$";

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultEmailValidator{TUser}" />.
        /// </summary>
        /// <param name="userStore">User store.</param>
        public DefaultEmailValidator(IUserStore<TUser> userStore)
        {
            _userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        }

        /// <summary>
        /// Validates email.
        /// </summary>
        /// <param name="manager">User manager.</param>
        /// <param name="user">User information.</param>
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();
            var email = user.Email;

            if (!string.IsNullOrWhiteSpace(email))
            {
                var match = Regex.IsMatch(email, EmailRegex);

                if (!match)
                {
                    errors.Add(new IdentityError {Description = string.Format(Resources.InvalidEmail, email)});
                }

                if (_userStore is IUserEmailStore<TUser> store)
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