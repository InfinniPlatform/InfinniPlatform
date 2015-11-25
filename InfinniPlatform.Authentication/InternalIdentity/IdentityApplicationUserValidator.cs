using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using InfinniPlatform.Authentication.Properties;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.InternalIdentity
{
    /// <summary>
    /// Проверяет корректность данных пользователей.
    /// </summary>
    internal sealed class IdentityApplicationUserValidator : IIdentityValidator<IdentityApplicationUser>
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
        /// Разрешать только пользователей с именами "^[A-Za-z0-9@_\\.]+$".
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

        public Task<IdentityResult> ValidateAsync(IdentityApplicationUser user)
        {
            return Task.Run(() =>
                            {
                                var errors = new List<string>();
                                ValidateUserName(user, errors);
                                ValidateEmail(user, errors);
                                ValidatePhoneNumber(user, errors);

                                return (errors.Count <= 0)
                                    ? IdentityResult.Success
                                    : IdentityResult.Failed(errors.ToArray());
                            });
        }

        private void ValidateUserName(IdentityApplicationUser user, List<string> errors)
        {
            var userName = user.UserName;

            // Имя пользователя является обязательным
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(Resources.UserNameCannotBeNullOrWhiteSpace);
            }
            // Проверка корректности имени пользователя
            else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(userName, "^[A-Za-z0-9@_\\.]+$", RegexOptions.Compiled))
            {
                errors.Add(string.Format(Resources.InvalidUserName, userName));
            }
            else
            {
                // Проверка уникальности

                var owner = _store.FindByUserNameAsync(userName).Result;

                if (owner != null && !string.Equals(owner.Id, user.Id))
                {
                    errors.Add(string.Format(Resources.DuplicateUserName, userName));
                }
            }
        }

        private void ValidateEmail(IdentityApplicationUser user, List<string> errors)
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
                    var owner = _store.FindByEmailAsync(email).Result;

                    if (owner != null && !string.Equals(owner.Id, user.Id))
                    {
                        errors.Add(string.Format(Resources.DuplicateEmail, email));
                    }
                }
            }
        }

        private void ValidatePhoneNumber(IdentityApplicationUser user, List<string> errors)
        {
            var phoneNumber = user.PhoneNumber;

            // Если номер телефона задан

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                // Проверка уникальности номера телефона

                if (RequireUniquePhoneNumber)
                {
                    var owner = _store.FindByPhoneNumberAsync(phoneNumber).Result;

                    if (owner != null && !string.Equals(owner.Id, user.Id))
                    {
                        errors.Add(string.Format(Resources.DuplicatePhoneNumber, phoneNumber));
                    }
                }
            }
        }
    }
}