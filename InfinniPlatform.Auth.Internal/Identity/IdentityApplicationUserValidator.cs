using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserStore;
using InfinniPlatform.Auth.Properties;

using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity
{
    /// <summary>
    /// Проверяет корректность данных пользователей.
    /// </summary>
    internal class IdentityApplicationUserValidator : IUserValidator<IdentityUser>
    {
        public IdentityApplicationUserValidator(UserStore<IdentityUser> store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }

            _store = store;

            AllowOnlyAlphanumericUserNames = true;
            RequireUniqueEmail = true;
            RequireUniquePhoneNumber = true;
        }

        private readonly UserStore<IdentityUser> _store;

        /// <summary>
        /// Разрешать только пользователей с именами @"^[A-Za-z0-9@_\+\-\.]+$".
        /// </summary>
        public bool AllowOnlyAlphanumericUserNames { get; set; }

        /// <summary>
        /// Требуется уникальность электронной почты (если задана).
        /// </summary>
        public bool RequireUniqueEmail { get; set; }

        /// <summary>
        /// Требуется уникальность номера телефона (если задан).
        /// </summary>
        public bool RequireUniquePhoneNumber { get; set; }


        public async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            var errors = new List<IdentityError>();
            await ValidateUserName(user, errors);
            await ValidateEmail(user, errors);
            await ValidatePhoneNumber(user, errors);

            var result = (errors.Count <= 0)
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray());

            return result;
        }

        private async Task ValidateUserName(IdentityUser user, List<IdentityError> errors)
        {
            var userName = user.UserName;

            // Имя пользователя является обязательным
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(new IdentityError
                           {
                               Description = Resources.UserNameCannotBeNullOrWhiteSpace
                           });
            }
            // Проверка корректности имени пользователя

            else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(userName, @"^[A-Za-z0-9@_\+\-\.]+$", RegexOptions.Compiled))
            {
                errors.Add(new IdentityError
                           {
                               Description = string.Format(Resources.InvalidUserName, userName)
                           });
            }
            else
            {
                // Проверка уникальности

                var owner = await _store.FindByNameAsync(userName, default(CancellationToken));

                if (owner != null && !string.Equals(owner.Id, user.Id))
                {
                    errors.Add(new IdentityError
                    {
                        Description = string.Format(Resources.DuplicateUserName, userName)
                    });
                }
            }
        }

        private async Task ValidateEmail(IdentityUser user, List<IdentityError> errors)
        {
            var email = user.Email;

            // Если Email задан
            if (!string.IsNullOrWhiteSpace(email))
            {
                // Проверка корректности Email

                try
                {
                    // TODO No MailAddress type in dotnet core.
                    //var mailAddress = new MailAddress(email);
                }
                catch (FormatException)
                {
                    errors.Add(new IdentityError {Description = string.Format(Resources.InvalidEmail, email)});
                    return;
                }

                // Проверка уникальности Email

                if (RequireUniqueEmail)
                {
                    var owner = await _store.FindByEmailAsync(email, default(CancellationToken));

                    if (owner != null && !string.Equals(owner.Id, user.Id))
                    {
                        errors.Add(new IdentityError { Description = string.Format(Resources.DuplicateEmail, email) });
                    }
                }
            }
        }

        private async Task ValidatePhoneNumber(IdentityUser user, List<IdentityError> errors)
        {
            var phoneNumber = user.PhoneNumber;

            // Если номер телефона задан

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                // Проверка уникальности номера телефона

                if (RequireUniquePhoneNumber)
                {
                    // TODO FindByPhoneNumberAsync implementation.
//                                        var owner = await _store.FindByPhoneNumberAsync(phoneNumber);
//                    
//                                        if (owner != null && !string.Equals(owner.Id, user.Id))
//                                        {
//                                            errors.Add(new IdentityError { Description = string.Format(Resources.DuplicatePhoneNumber, phoneNumber) });
//                                        }
                }
            }
        }
    }
}