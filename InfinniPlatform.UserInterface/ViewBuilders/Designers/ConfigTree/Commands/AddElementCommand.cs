using System.Linq;
using InfinniPlatform.UserInterface.AppHost;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
    internal sealed class AddElementCommand : CommandBase<object>
    {
        private readonly ConfigElementNodeBuilder _builder;
        private readonly string _elementEditor;
        private readonly ConfigElementNode _elementNode;
        private readonly string _elementType;

        public AddElementCommand(ConfigElementNodeBuilder builder, ConfigElementNode elementNode, string elementEditor,
            string elementType)
        {
            _builder = builder;
            _elementNode = elementNode;
            _elementEditor = elementEditor;
            _elementType = elementType;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            var parentNode = _elementNode;

            if (_elementNode.ElementChildrenTypes.Length > 1)
            {
                // Поиск контейнера элементов заданного типа (для обновления при сохранении)
                parentNode = _elementNode.Nodes.FirstOrDefault(i => (i.ElementChildrenTypes != null)
                                                                    && (i.ElementChildrenTypes.Length == 1)
                                                                    && (i.ElementChildrenTypes.Contains(_elementType)))
                             ?? _elementNode;
            }

            //TODO: Необходимо переосмыслить механизм указания значения версии для новой конфигурации
            var version = parentNode.Version;
            if (_elementType == "Configuration" && parentNode.Version == null)
            {
                version = StaticMetadata.CreateVersion();
            }

            _builder.EditPanel.AddElement(_elementEditor,
                parentNode.GetNodePath(),
                parentNode.ConfigId,
                parentNode.DocumentId,
                version,
                _elementType,
                () => RefreshNode(parentNode));
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