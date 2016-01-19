using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Metadata
{
    internal sealed class ConfigurationMetadata : IConfigurationMetadata
    {
        public ConfigurationMetadata(string configuration)
        {
            Configuration = configuration;
        }

        public string Configuration { get; }

        // Menu

        private readonly Dictionary<string, dynamic> _menu = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

        public void AddMenu(IEnumerable<dynamic> menuList)
        {
            if (menuList != null)
            {
                foreach (var menu in menuList)
                {
                    var menuName = menu.Name ?? string.Empty;

                    _menu[menuName] = menu;
                }
            }
        }

        public IEnumerable<string> GetMenuNames()
        {
            return _menu.Keys;
        }

        public dynamic GetMenu(string menuName)
        {
            dynamic menu;

            return !string.IsNullOrEmpty(menuName)
                   && _menu.TryGetValue(menuName, out menu)
                ? menu
                : null;
        }


        // Register

        private readonly Dictionary<string, dynamic> _registers = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

        public void AddRegisters(IEnumerable<dynamic> registerList)
        {
            if (registerList != null)
            {
                foreach (var register in registerList)
                {
                    var registerName = register.Name ?? string.Empty;

                    _registers[registerName] = register;
                }
            }
        }

        public IEnumerable<string> GetRegisterNames()
        {
            return _registers.Keys;
        }

        public dynamic GetRegister(string registerName)
        {
            dynamic register;

            return !string.IsNullOrEmpty(registerName)
                   && _registers.TryGetValue(registerName, out register)
                ? register
                : null;
        }

        // Document

        private readonly Dictionary<string, DocumentMetadata> _documents = new Dictionary<string, DocumentMetadata>(StringComparer.OrdinalIgnoreCase);

        public void AddDocuments(IEnumerable<dynamic> documentList)
        {
            if (documentList != null)
            {
                foreach (var document in documentList)
                {
                    var documentType = document.Name ?? string.Empty;
                    var documentSchema = document.Schema ?? new DynamicWrapper();
                    var documentEvents = document.Events ?? new DynamicWrapper();

                    _documents[documentType] = new DocumentMetadata { Schema = documentSchema, Events = documentEvents };
                }
            }
        }

        public IEnumerable<string> GetDocumentNames()
        {
            return _documents.Keys;
        }

        public dynamic GetDocumentSchema(string documentName)
        {
            DocumentMetadata document;

            return !string.IsNullOrEmpty(documentName)
                   && _documents.TryGetValue(documentName, out document)
                ? document.Schema
                : null;
        }

        public dynamic GetDocumentEvents(string documentName)
        {
            DocumentMetadata document;

            return !string.IsNullOrEmpty(documentName)
                   && _documents.TryGetValue(documentName, out document)
                ? document.Events
                : null;
        }

        // Action

        public void AddActions(string documentName, IEnumerable<dynamic> actions)
        {
            if (!string.IsNullOrEmpty(documentName) && actions != null)
            {
                DocumentMetadata document;

                if (_documents.TryGetValue(documentName, out document))
                {
                    document.AddActions(actions);
                }
            }
        }

        public IEnumerable<string> GetActionNames(string documentName)
        {
            DocumentMetadata document;

            return !string.IsNullOrEmpty(documentName)
                   && _documents.TryGetValue(documentName, out document)
                ? document.GetActionNames()
                : Enumerable.Empty<string>();
        }

        public dynamic GetAction(string documentName, string actionName)
        {
            DocumentMetadata document;

            return !string.IsNullOrEmpty(documentName)
                   && !string.IsNullOrEmpty(actionName)
                   && _documents.TryGetValue(documentName, out document)
                ? document.GetAction(actionName)
                : null;
        }

        // View

        public void AddViews(string documentName, IEnumerable<dynamic> views)
        {
            if (!string.IsNullOrEmpty(documentName) && views != null)
            {
                DocumentMetadata document;

                if (_documents.TryGetValue(documentName, out document))
                {
                    document.AddViews(views);
                }
            }
        }

        public IEnumerable<string> GetViewNames(string documentName)
        {
            DocumentMetadata document;

            return !string.IsNullOrEmpty(documentName)
                   && _documents.TryGetValue(documentName, out document)
                ? document.GetViewNames()
                : Enumerable.Empty<string>();
        }

        public dynamic GetView(string documentName, string viewName)
        {
            DocumentMetadata document;

            return !string.IsNullOrEmpty(documentName)
                   && !string.IsNullOrEmpty(viewName)
                   && _documents.TryGetValue(documentName, out document)
                ? document.GetView(viewName)
                : null;
        }

        // PrintView

        public void AddPrintViews(string documentName, IEnumerable<dynamic> printViews)
        {
            if (!string.IsNullOrEmpty(documentName) && printViews != null)
            {
                DocumentMetadata document;

                if (_documents.TryGetValue(documentName, out document))
                {
                    document.AddPrintViews(printViews);
                }
            }
        }

        public IEnumerable<string> GetPrintViewNames(string documentName)
        {
            DocumentMetadata document;

            return !string.IsNullOrEmpty(documentName)
                   && _documents.TryGetValue(documentName, out document)
                ? document.GetPrintViewNames()
                : Enumerable.Empty<string>();
        }

        public dynamic GetPrintView(string documentName, string printViewName)
        {
            DocumentMetadata document;

            return !string.IsNullOrEmpty(documentName)
                   && !string.IsNullOrEmpty(printViewName)
                   && _documents.TryGetValue(documentName, out document)
                ? document.GetPrintView(printViewName)
                : null;
        }


        private sealed class DocumentMetadata
        {
            public dynamic Schema;

            public dynamic Events;

            // Action

            private readonly Dictionary<string, dynamic> _actions = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

            public void AddActions(IEnumerable<dynamic> actionList)
            {
                if (actionList != null)
                {
                    foreach (var action in actionList)
                    {
                        var actionName = action.Id ?? string.Empty;

                        _actions[actionName] = action;
                    }
                }
            }

            public IEnumerable<string> GetActionNames()
            {
                return _actions.Keys;
            }

            public dynamic GetAction(string actionName)
            {
                dynamic action;

                return !string.IsNullOrEmpty(actionName)
                       && _actions.TryGetValue(actionName, out action)
                    ? action
                    : null;
            }

            // View

            private readonly Dictionary<string, dynamic> _views = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

            public void AddViews(IEnumerable<dynamic> viewList)
            {
                if (viewList != null)
                {
                    foreach (var view in viewList)
                    {
                        var viewName = view.Name ?? string.Empty;

                        _views[viewName] = view;
                    }
                }
            }

            public IEnumerable<string> GetViewNames()
            {
                return _views.Keys;
            }

            public dynamic GetView(string viewName)
            {
                dynamic view;

                return !string.IsNullOrEmpty(viewName)
                       && _views.TryGetValue(viewName, out view)
                    ? view
                    : null;
            }

            // PrintView

            private readonly Dictionary<string, dynamic> _printViews = new Dictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

            public void AddPrintViews(IEnumerable<dynamic> printViewList)
            {
                if (printViewList != null)
                {
                    foreach (var printView in printViewList)
                    {
                        var printViewName = printView.Name ?? string.Empty;

                        _printViews[printViewName] = printView;
                    }
                }
            }

            public IEnumerable<string> GetPrintViewNames()
            {
                return _printViews.Keys;
            }

            public dynamic GetPrintView(string printViewName)
            {
                dynamic printView;

                return !string.IsNullOrEmpty(printViewName)
                       && _printViews.TryGetValue(printViewName, out printView)
                    ? printView
                    : null;
            }
        }
    }
}