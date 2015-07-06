using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Hosting.Implementation.ExtensionPointHandling
{
    public sealed class ExtensionPointHandlerInstance : IExtensionPointHandlerInstance
    {
        private readonly List<ExtensionPointValue> _extensionPoints = new List<ExtensionPointValue>();
        private readonly string _handlerInstanceName;

        public ExtensionPointHandlerInstance(string handlerInstanceName)
        {
            _handlerInstanceName = handlerInstanceName;
        }

        public string HandlerInstanceName
        {
            get { return _handlerInstanceName; }
        }

        public IExtensionPointHandlerInstance RegisterExtensionPoint(string extensionPointTypeName,
            string stateMachineReference)
        {
            _extensionPoints.Add(new ExtensionPointValue(extensionPointTypeName, stateMachineReference));
            return this;
        }

        public ExtensionPointValue GetExtensionPoint(string extensionPointTypeName)
        {
            return
                _extensionPoints.FirstOrDefault(
                    ex => ex.TypeName.ToLowerInvariant() == extensionPointTypeName.ToLowerInvariant());
        }
    }
}