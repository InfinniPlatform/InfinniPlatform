using System;

namespace InfinniPlatform.Aspects
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AspectAttribute : Attribute
    {
        public AspectAttribute(Type interceptorType)
        {
            InterceptorType = interceptorType;
        }

        public Type InterceptorType { get; set; }
    }
}