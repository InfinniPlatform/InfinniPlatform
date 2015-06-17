using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Factories
{
    internal sealed class ContextRegistration
    {
        private readonly Type _typeRegistration;

        private readonly Func<string, object> _instanceConstructor;

        public ContextRegistration(Type typeRegistration, Func<string,object> instanceConstructor )
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
