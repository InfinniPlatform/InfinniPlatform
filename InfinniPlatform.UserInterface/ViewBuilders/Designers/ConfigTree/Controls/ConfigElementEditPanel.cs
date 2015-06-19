using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Data;
using InfinniPlatform.UserInterface.ViewBuilders.LinkViews;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Controls
{
    internal sealed class ConfigElementEditPanel : IConfigElementEditPanel
    {
        private readonly List<IDataSource> _dataSources = new List<IDataSource>();
        private readonly object _layoutPanel;
        private readonly Func<string, string, LinkView> _linkViewFactory;

        public ConfigElementEditPanel(object layoutPanel, Func<string, string, LinkView> linkViewFactory)
        {
            _layoutPanel = layoutPanel;
            _linkViewFactory = linkViewFactory;
        }

        public object LayoutPanel
        {
            get { return _layoutPanel; }
        }

        public void AddElement(string elementEditor, string elementPath, string configId, string documentId,
            string version, string elementType, Action onSave)
        {
            ShowEditorView(elementEditor, elementPath, configId, documentId, version, elementType, null, null, onSave);
        }

        public void AddElement(string elementEditor, string elementPath, string configId, string documentId,
            string version, string elementType, object template, Action onSave)
        {
            ShowEditorView(elementEditor, elementPath, configId, documentId, version, elementType, null, template,
                onSave);
        }

        public void EditElement(string elementEditor, string elementPath, string configId, string documentId,
            string version, string elementType, string elementId, Action onSave)
        {
            ShowEditorView(elementEditor, elementPath, configId, documentId, version, elementType, elementId, null,
                onSave);
        }

        public void EditElement(string elementEditor, string elementPath, string configId, string documentId,
            string version, string elementType, string elementId, object template, Action onSave)
        {
            ShowEditorView(elementEditor, elementPath, configId, documentId, version, elementType, elementId, template,
                onSave);
        }

        public void DeleteElement(string configId, string documentId, string version, string elementType,
            string elementId)
        {
            var closeEditors = new List<View>();

            // Редакторы, которые нужно закрыть при удалении элемента
            foreach (var dataSource in _dataSources)
            {
                if ((elementType == MetadataType.Configuration
                     && dataSource.GetConfigId() == configId)
                    || (elementType == MetadataType.Document
                        && dataSource.GetConfigId() == configId
                        && dataSource.GetDocumentId() == documentId
                        && dataSource.GetVersion() == version
                        )
                    || (dataSource.GetConfigId() == configId
                        && dataSource.GetDocumentId() == documentId
                        && dataSource.GetIdFilter() == elementId
                        && dataSource.GetVersion() == version))
                {
                    closeEditors.Add(dataSource.GetView());
                }
            }

            // Закрытие всех связанных редакторов
            foreach (var editor in closeEditors)
            {
                editor.Close(true);
            }
        }

        public void CloseAll()
        {
            foreach (var dataSource in _dataSources.ToArray())
            {
                dataSource.GetView().Close(true);
            }
        }

        private void ShowEditorView(string elementEditor, string elementPath, string configId, string documentId,
            string version, string elementType, string elementId, object template, Action onSave)
        {
            var editorId = GetViewKey(elementEditor, configId, documentId, elementId);

            ViewHelper.ShowView(editorId,
                () => _linkViewFactory(elementType, elementEditor),
                dataSource => OnInitView(dataSource, elementPath, configId, documentId, version, elementId, template),
                dataSource => OnAcceptView(onSave));
        }

        private static string GetViewKey(string elementEditor, string configId, string documentId, string elementId)
        {
            return string.IsNullOrEmpty(elementId)
                ? null
                : string.Format("{0}/{1}/{2}/{3}", elementEditor, configId, documentId, elementId);
        }

        private void OnInitView(IDataSource dataSource, string elementPath, string configId, string documentId,
            string version, string elementId, object template)
        {
            if (dataSource != null)
            {
                // Установка подсказки для редактора

                var view = dataSource.GetView();
                view.SetToolTip(elementPath);

                // Настройка источника данных открываемого редактора

                dataSource.SuspendUpdate();
                dataSource.SetEditMode();
                dataSource.SetConfigId(configId);
                dataSource.SetDocumentId(documentId);
                dataSource.SetIdFilter(elementId);
                dataSource.SetVersion(version);
                dataSource.ResumeUpdate();

                if (template != null)
                {
                    dataSource.SetSelectedItem(template);
                    dataSource.ResetModified(template);
                }

                // Регистрация источника данных, чтобы можно было найти связанные редакторы

                ScriptDelegate onClose = null;

                onClose = (c, a) =>
                {
                    view.OnClosed -= onClose;
                    _dataSources.Remove(dataSource);
                };

                view.OnClosed += onClose;
                _dataSources.Add(dataSource);
            }
        }

        private static void OnAcceptView(Action onSave)
        {
            if (onSave != null)
            {
                onSave();
            }
        }
    }
}