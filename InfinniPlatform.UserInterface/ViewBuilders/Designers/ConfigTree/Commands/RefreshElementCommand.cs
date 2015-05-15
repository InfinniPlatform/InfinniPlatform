using System.Collections.Generic;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
	sealed class RefreshElementCommand : CommandBase<bool>
	{
		private readonly ConfigElementNodeBuilder _builder;
		private readonly ICollection<ConfigElementNode> _elements;
		private readonly ConfigElementNode _elementNode;
		private readonly string _elementType;

		public RefreshElementCommand(ConfigElementNodeBuilder builder, ICollection<ConfigElementNode> elements, ConfigElementNode elementNode, string elementType)
		{
			_builder = builder;
			_elements = elements;
			_elementNode = elementNode;
			_elementType = elementType;
		}

		public override bool CanExecute(bool reload)
		{
			return true;
		}

		public override void Execute(bool reload)
		{
			if (reload || !_elementNode.IsLoaded)
			{
				// Удаление дочерних элементов в визуальном дереве
				CommandHelper.RemoveChildNodes(_elements, _elementNode);

				// Загрузка актуального списка метаданных дочерних элементов
				var metadataProvider = CommandHelper.GetMetadataProvider(_elementNode, _elementType);
				var elementMetadata = metadataProvider.GetItems(null, 0, 1000);

				// Обновление списка дочерних элементов в визуальном дереве
				_builder.BuildElements(_elements, _elementNode, elementMetadata, _elementType);

				_elementNode.IsLoaded = true;
			}
		}
	}
}