using System;

namespace InfinniPlatform.Factories
{
    internal sealed class ContextRegistration
    {
        private readonly Func<string, object> _instanceConstructor;
        private readonly Type _typeRegistration;

        public ContextRegistration(Type typeRegistration, Func<string, object> instanceConstructor)
        {
            _typeRegistration = typeRegistration;
            _instanceConstructor = instanceConstructor;
        }

        public bool IsTypeOf(Type type)
        {
            return _typeRegistration.IsAssignableFrom(type);
        }

        public object GetInstance(string version)
        {
            return _instanceConstructor(version);
        }
    }
}