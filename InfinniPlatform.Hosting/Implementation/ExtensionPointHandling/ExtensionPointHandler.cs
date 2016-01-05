using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Hosting.Implementation.ExtensionPointHandling
{
    public sealed class ExtensionPointHandler : IExtensionPointHandler
    {
        private VerbType _verbType = VerbType.Get;

        private readonly IList<IExtensionPointHandlerInstance> _actionHandlerInstances =
            new List<IExtensionPointHandlerInstance>();

        private readonly string _actionName;
        private readonly IList<ExtensionPointType> _extensionPointTypes = new List<ExtensionPointType>();

        public ExtensionPointHandler(string actionName)
        {
            _actionName = actionName;
            //AddHandlerInstance(new ExtensionPointHandlerInstance(actionName));
        }

        public string ActionName
        {
            get { return _actionName; }
        }

        public VerbType VerbType
        {
            get { return _verbType; }
        }

        public IExtensionPointHandler AsVerb(VerbType verbType)
        {
            _verbType = verbType;
            return this;
        }

        public IExtensionPointHandler AddExtensionPoint(string extensionPointName,
            ContextTypeKind extensionPointContextTypeKind)
        {
            _extensionPointTypes.Add(new ExtensionPointType
            {
                Name = extensionPointName,
                ContextTypeKind = extensionPointContextTypeKind
            });
            return this;
        }

        public void AddHandlerInstance(IExtensionPointHandlerInstance extensionPointHandlerInstance)
        {
            var existingHandler =
                _actionHandlerInstances.FirstOrDefault(
                    a =>
                        a.HandlerInstanceName.ToLowerInvariant() ==
                        extensionPointHandlerInstance.HandlerInstanceName.ToLowerInvariant());
            if (existingHandler != null)
            {
                _actionHandlerInstances.Remove(existingHandler);
            }

            _actionHandlerInstances.Add(extensionPointHandlerInstance);
        }

        public IExtensionPointHandlerInstance GetHandlerInstance(string actionHandlerInstanceName)
        {
            return
                _actionHandlerInstances.OfType<ExtensionPointHandlerInstance>().FirstOrDefault(
                    h => h.HandlerInstanceName.ToLowerInvariant() == actionHandlerInstanceName.ToLowerInvariant());
        }

        public IEnumerable<string> GetInstanceNames()
        {
            return _actionHandlerInstances.Select(a => a.HandlerInstanceName).ToList();
        }
    }

    public static class ExtensionPointHandlerExtensions
    {
        public static IServiceRegistration RegisterHandlerInstance(this IServiceRegistration serviceRegistration,
            string handlerName, Action<IExtensionPointHandlerInstance> initAction = null)
        {
            var actionHandlerInstance = new ExtensionPointHandlerInstance(handlerName);
            if (serviceRegistration.ExtensionPointHandler == null)
            {
                serviceRegistration.SetExtensionPointHandler(method => new ExtensionPointHandler(method));
            }

            serviceRegistration.ExtensionPointHandler.AddHandlerInstance(actionHandlerInstance);
            if (initAction != null)
            {
                initAction(actionHandlerInstance);
            }
            return serviceRegistration;
        }
    }
}