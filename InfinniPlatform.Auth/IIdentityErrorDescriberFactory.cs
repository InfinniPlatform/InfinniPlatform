using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Factory for creating <see cref="Microsoft.AspNetCore.Identity.IdentityErrorDescriber" /> instance.
    /// </summary>
    public interface IIdentityErrorDescriberFactory
    {
        /// <summary>
        /// Returns <see cref="IdentityErrorDescriber"/> instance.
        /// </summary>
        /// <param name="resolver">Application container resolver.</param>
        IdentityErrorDescriber Get(IContainerResolver resolver);
    }
}