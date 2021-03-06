﻿using System;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Accessors for getting <see cref="IServiceProvider" /> instances in current scope.
    /// </summary>
    public interface IServiceProviderAccessor
    {
        /// <summary>
        /// Returns service provider for current scope.
        /// </summary>
        /// <returns></returns>
        IServiceProvider GetProvider();
    }
}