using System;

namespace InfinniPlatform.Aspects
{
    /// <summary>
    /// Enables method interception for current class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AspectAttribute : Attribute
    {
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