using System.Collections.Generic;
using InfinniPlatform.UserInterface.Properties;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
    internal sealed class DeleteElementCommand : CommandBase<object>
    {
        private readonly ConfigElementNodeBuilder _builder;
        private readonly ConfigElementNode _elementNode;
        private readonly ICollection<ConfigElementNode> _elements;

        public DeleteElementCommand(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements,
            ConfigElementNode elementNode)
        {
            _builder = builder;
            _elements = elements;
            _elementNode = elementNode;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            if (CommonHelper.AcceptQuestionMessage(Resources.DeleteConfigElementQuestion, _elementNode))
            {
                // Удаление метаданных элемента
                var metadataProvider = CommandHelper.GetMetadataProvider(_elementNode, _elementNode.ElementType);
                metadataProvider.DeleteItem(_elementNode.ElementId);

                // Удаление элемента из визуального дерева
                CommandHelper.RemoveNode(_elements, _elementNode);

                // Закрытие зависимых редакторов, если они открыты
                if (_builder.EditPanel != null)
                {
                    _builder.EditPanel.DeleteElement(_elementNode.ConfigId, _elementNode.DocumentId,
                        _elementNode.Version, _elementNode.ElementType, _elementNode.ElementId);
                }
            }
        }
    }
}