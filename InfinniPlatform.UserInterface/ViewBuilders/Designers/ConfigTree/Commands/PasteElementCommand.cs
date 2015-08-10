using System.Linq;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
    internal sealed class PasteElementCommand : CommandBase<object>
    {
        private readonly string _server;
        private readonly int _port;
        private readonly string _version;
        private readonly ConfigElementNodeBuilder _builder;
        private readonly string _elementEditor;
        private readonly ConfigElementNode _elementNode;

        public PasteElementCommand(string server, int port, string version)
        {
            _server = server;
            _port = port;
            _version = version;
        }

        public PasteElementCommand(ConfigElementNodeBuilder builder, ConfigElementNode elementNode, string elementEditor)
        {
            _builder = builder;
            _elementNode = elementNode;
            _elementEditor = elementEditor;
        }

        public override bool CanExecute(object parameter)
        {
            var copyElement = CommandHelper.Clipboard;

            return (copyElement != null)
                   && !string.IsNullOrEmpty(copyElement.ElementId)
                   && !string.IsNullOrEmpty(copyElement.ElementType)
                   && (_builder.EditPanel != null)
                   && (_elementNode.ElementChildrenTypes != null)
                   && (_elementNode.ElementChildrenTypes.Length > 0)
                   && (_elementNode.ElementChildrenTypes.Contains(copyElement.ElementType));
        }

        public override void Execute(object parameter)
        {
            var parentNode = _elementNode;
            var copyElement = CommandHelper.Clipboard;

            if (_elementNode.ElementChildrenTypes.Length > 1)
            {
                // Поиск контейнера элементов заданного типа (для обновления при сохранении)
                parentNode = _elementNode.Nodes.FirstOrDefault(i => (i.ElementChildrenTypes != null)
                                                                    && (i.ElementChildrenTypes.Length == 1)
                                                                    &&
                                                                    (i.ElementChildrenTypes.Contains(
                                                                        copyElement.ElementType)))
                             ?? _elementNode;
            }

            // Копирование метаданных элемента
            var metadataProvider = CommandHelper.GetMetadataProvider(copyElement, copyElement.ElementType, _server, _port, _version);
            var elementTemplate = metadataProvider.CloneItem(copyElement.ElementId);

            _builder.EditPanel.AddElement(_elementEditor,
                parentNode.GetNodePath(),
                parentNode.ConfigId,
                parentNode.DocumentId, parentNode.Version, copyElement.ElementType, elementTemplate,
                () => RefreshNode(parentNode));

            CommandHelper.Clipboard = null;
        }

        private static void RefreshNode(ConfigElementNode parentNode)
        {
            if (parentNode.RefreshCommand.CanExecute(true))
            {
                parentNode.RefreshCommand.Execute(true);
            }
        }
    }
}