using System;

namespace InfinniPlatform.Core.Factories
{
    internal sealed class ContextRegistration
    {
        private readonly Func<object> _instanceConstructor;
        private readonly Type _typeRegistration;

        public ContextRegistration(Type typeRegistration, Func<object> instanceConstructor)
        {
            _typeRegistration = typeRegistration;
            _instanceConstructor = instanceConstructor;
        }

        public bool IsTypeOf(Type type)
        {
            return _typeRegistration.IsAssignableFrom(type);
        }

        public object GetInstance()
        {
            return _instanceConstructor();
        }
    }
}