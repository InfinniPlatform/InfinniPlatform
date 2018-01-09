using InfinniPlatform.IoC;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth
{
    /// <summary>
    /// Factory for creating <see cref="ILookupNormalizer"/> instance.
    /// </summary>
    public interface ILookupNormalizerFactory
    {
        /// <summary>
        /// Returns <see cref="ILookupNormalizer"/> instance.
        /// </summary>
        /// <param name="resolver">Application container resolver.</param>
        ILookupNormalizer Get(IContainerResolver resolver);
    }
}