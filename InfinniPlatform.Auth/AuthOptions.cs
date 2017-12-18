using System;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Authentication options from configuration.
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Name of option section in configuration file.
        /// </summary>
        public const string SectionName = "auth";

        /// <summary>
        /// Default user cache invalidation timeout (in minutes).
        /// </summary>
        public const int DefaultUserCacheTimeout = 30;

        /// <summary>
        /// Default instance of <see cref="AuthOptions" />.
        /// </summary>
        public static readonly AuthOptions Default = new AuthOptions();


        /// <summary>
        /// Initializes a new instance of <see cref="AuthOptions" />.
        /// </summary>
        public AuthOptions()
        {
            UserCacheTimeout = DefaultUserCacheTimeout;
        }


        /// <summary>
        /// User cache invalidation timeout (in minutes).
        /// </summary>
        public int UserCacheTimeout { get; set; }

        /// <summary>
        /// Factory for creating <see cref="IUserStore{TUser}" /> instance.
        /// </summary>
        public IUserStoreFactory UserStoreFactory { get; set; }

        /// <summary>
        /// ASP.NET Identity options.
        /// </summary>
        public Action<IdentityOptions> IdentityOptions { get; set; }

        /// <summary>
        /// Factory for creating <see cref="IPasswordHasher{TUser}" /> instance.
        /// </summary>
        public IPasswordHasherFactory PasswordHasherFactory { get; set; }

        /// <summary>
        /// Factory for creating <see cref="IUserValidator{TUser}" /> instances.
        /// </summary>
        public IUserValidatorsFactory UserValidatorsFactory { get; set; }

        /// <summary>
        /// Factory for creating <see cref="IPasswordValidator{TUser}" /> instances.
        /// </summary>
        public IPasswordValidatorsFactory PasswordValidatorsFactory { get; set; }

        /// <summary>
        /// Factory for creating <see cref="ILookupNormalizer" /> instance.
        /// </summary>
        public ILookupNormalizerFactory LookupNormalizerFactory { get; set; }

        /// <summary>
        /// Factory for creating  <see cref="Microsoft.AspNetCore.Identity.IdentityErrorDescriber" /> instance.
        /// </summary>
        public IIdentityErrorDescriberFactory IdentityErrorDescriber { get; set; }
    }
}