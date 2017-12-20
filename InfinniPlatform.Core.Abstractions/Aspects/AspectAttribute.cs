using System;

namespace InfinniPlatform.Aspects
{
    /// <summary>
    /// Enables method interception for current class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AspectAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AspectAttribute" />.
        /// </summary>
        /// <param name="interceptorType">Type of interceptor class.</param>
        public AspectAttribute(Type interceptorType)
        {
            InterceptorType = interceptorType;
        }

        /// <summary>
        /// Type of <see cref="IMethodInterceptor"/> implementation.
        /// </summary>
        public Type InterceptorType { get; set; }
    }
}