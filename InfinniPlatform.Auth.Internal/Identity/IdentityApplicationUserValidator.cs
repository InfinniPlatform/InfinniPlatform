using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using InfinniPlatform.Auth.Internal.Properties;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Auth.Internal.Identity
{
    /// <summary>
    /// Проверяет корректность данных пользователей.
    /// </summary>
    internal class IdentityApplicationUserValidator : IIdentityValidator<IdentityApplicationUser>
    {
        public IdentityApplicationUserValidator(IdentityApplicationUserStore store)
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

        private readonly IdentityApplicationUserStore _store;

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

        public async Task<IdentityResult> ValidateAsync(IdentityApplicationUser user)
        {
            var errors = new List<string>();
            await ValidateUserName(user, errors);
            await ValidateEmail(user, errors);
            await ValidatePhoneNumber(user, errors);

            var result = (errors.Count <= 0)
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray());

            return result;
        }

        private async Task ValidateUserName(IdentityApplicationUser user, List<string> errors)
        {
            var userName = user.UserName;

            // Имя пользователя является обязательным
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(Resources.UserNameCannotBeNullOrWhiteSpace);
            }
            // Проверка корректности имени пользователя

            else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(userName, @"^[A-Za-z0-9@_\+\-\.]+$", RegexOptions.Compiled))
            {
                errors.Add(string.Format(Resources.InvalidUserName, userName));
            }
            else
            {
                // Проверка уникальности

                var owner = await _store.FindByUserNameAsync(userName);

                if (owner != null && !string.Equals(owner.Id, user.Id))
                {
                    errors.Add(string.Format(Resources.DuplicateUserName, userName));
                }
            }
        }

        private async Task ValidateEmail(IdentityApplicationUser user, List<string> errors)
        {
            var email = user.Email;

            // Если Email задан
            if (!string.IsNullOrWhiteSpace(email))
            {
                // Проверка корректности Email

                try
                {
                    // ReSharper disable UnusedVariable

                    var mailAddress = new MailAddress(email);

                    // ReSharper restore UnusedVariable
                }
                catch (FormatException)
                {
                    errors.Add(string.Format(Resources.InvalidEmail, email));
                    return;
                }

                // Проверка уникальности Email

                if (RequireUniqueEmail)
                {
                    var owner = await _store.FindByEmailAsync(email);

                    if (owner != null && !string.Equals(owner.Id, user.Id))
                    {
                        errors.Add(string.Format(Resources.DuplicateEmail, email));
                    }
                }
            }
        }

        private async Task ValidatePhoneNumber(IdentityApplicationUser user, List<string> errors)
        {
            var phoneNumber = user.PhoneNumber;

            // Если номер телефона задан

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                // Проверка уникальности номера телефона

                if (RequireUniquePhoneNumber)
                {
                    var owner = await _store.FindByPhoneNumberAsync(phoneNumber);

                    if (owner != null && !string.Equals(owner.Id, user.Id))
                    {
                        errors.Add(string.Format(Resources.DuplicatePhoneNumber, phoneNumber));
                    }
                }
            }
        }
    }
}