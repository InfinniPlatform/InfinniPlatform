using System;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree
{
    public interface IConfigElementEditPanel
    {
        object LayoutPanel { get; }

        void AddElement(string elementEditor, string elementPath, string configId, string documentId, string version,
            string elementType, Action onSave);

        void AddElement(string elementEditor, string elementPath, string configId, string documentId, string version,
            string elementType, object template, Action onSave);

        void EditElement(string elementEditor, string elementPath, string configId, string documentId, string version,
            string elementType, string elementId, Action onSave);

        void EditElement(string elementEditor, string elementPath, string configId, string documentId, string version,
            string elementType, string elementId, object template, Action onSave);

        void DeleteElement(string configId, string documentId, string version, string elementType, string elementId);
        void CloseAll();
    }
}