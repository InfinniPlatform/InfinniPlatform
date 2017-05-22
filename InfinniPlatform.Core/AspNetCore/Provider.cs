// Decompiled with JetBrains decompiler
// Type: Autofac.Extensions.DependencyInjection.AutofacServiceProvider
// Assembly: Autofac.Extensions.DependencyInjection, Version=4.1.0.0, Culture=neutral, PublicKeyToken=17863af14b0044da
// MVID: 859EB718-334F-4DA7-9637-687A167BE291
// Assembly location: C:\Users\s.pevnev\.nuget\packages\autofac.extensions.dependencyinjection\4.1.0\lib\netstandard1.1\Autofac.Extensions.DependencyInjection.dll

using System;
using System.Net.Http.Headers;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.AspNetCore

{
    /// <summary>
    /// Autofac implementation of the ASP.NET Core <see cref="T:System.IServiceProvider" />.
    /// </summary>
    /// <seealso cref="T:System.IServiceProvider" />
    /// <seealso cref="T:Microsoft.Extensions.DependencyInjection.ISupportRequiredService" />
    public class Provider : IServiceProvider, ISupportRequiredService
    {
        private readonly IComponentContext _componentContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Autofac.Extensions.DependencyInjection.AutofacServiceProvider" /> class.
        /// </summary>
        /// <param name="componentContext">
        /// The component context from which services should be resolved.
        /// </param>
        public Provider(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        /// <summary>Gets the service object of the specified type.</summary>
        /// <param name="serviceType">
        /// An object that specifies the type of service object to get.
        /// </param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" />; or <see langword="null" />
        /// if there is no service object of type <paramref name="serviceType" />.
        /// </returns>
        public object GetService(Type serviceType)
        {
            if (serviceType.FullName.Contains("IServiceScopeFactory"))
            {
                return new MyScopeFactory(this);
            }

            return _componentContext.ResolveOptional(serviceType);
        }

        /// <summary>
        /// Gets service of type <paramref name="serviceType" /> from the
        /// <see cref="T:Autofac.Extensions.DependencyInjection.AutofacServiceProvider" /> and requires it be present.
        /// </summary>
        /// <param name="serviceType">
        /// An object that specifies the type of service object to get.
        /// </param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" />.
        /// </returns>
        /// <exception cref="T:Autofac.Core.Registration.ComponentNotRegisteredException">
        /// Thrown if the <paramref name="serviceType" /> isn't registered with the container.
        /// </exception>
        /// <exception cref="T:Autofac.Core.DependencyResolutionException">
        /// Thrown if the object can't be resolved from the container.
        /// </exception>
        public object GetRequiredService(Type serviceType)
        {
            if (serviceType.FullName.Contains("IServiceScopeFactory"))
            {
                return new MyScopeFactory(this);
            }

            return _componentContext.Resolve(serviceType);
        }
    }


    public class MyScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceProvider _p;

        public MyScopeFactory(IServiceProvider p)
        {
            Console.WriteLine(nameof(MyScopeFactory));
            _p = p;
        }

        public IServiceScope CreateScope()
        {
            return new MyScope(_p);
        }
    }

    public class MyScope : IServiceScope
    {
        private readonly IServiceProvider _p;

        public MyScope(IServiceProvider p)
        {
            Console.WriteLine(nameof(MyScope));
            _p = p;
        }

        public void Dispose()
        {
            
        }

        public IServiceProvider ServiceProvider => _p;
    }
}