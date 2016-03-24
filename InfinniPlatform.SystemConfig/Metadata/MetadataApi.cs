using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Metadata
{
    internal sealed class MetadataApi : IMetadataApi
    {
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
                    var documentIndexes = document.Indexes ?? new List<dynamic>();

                    _documents[documentType] = new DocumentMetadata { Schema = documentSchema, Events = documentEvents, Indexes = documentIndexes };
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

        public IEnumerable<object> GetDocumentIndexes(string documentName)
        {
            DocumentMetadata document;

            return !string.IsNullOrEmpty(documentName)
                   && _documents.TryGetValue(documentName, out document)
                ? document.Indexes
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


        private sealed class DocumentMetadata
        {
            public dynamic Schema;

            public dynamic Events;

            public IEnumerable<object> Indexes;
        }
    }
}