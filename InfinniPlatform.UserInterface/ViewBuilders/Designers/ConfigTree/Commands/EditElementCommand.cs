namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
	sealed class EditElementCommand : CommandBase<object>
	{
		private readonly ConfigElementNodeBuilder _builder;
		private readonly ConfigElementNode _elementNode;
		private readonly string _elementEditor;

		public EditElementCommand(ConfigElementNodeBuilder builder, ConfigElementNode elementNode, string elementEditor)
		{
			_builder = builder;
			_elementNode = elementNode;
			_elementEditor = elementEditor;
		}

		public override bool CanExecute(object parameter)
		{
			return true;
		}

		public override void Execute(object parameter)
		{
			var parentNode = _elementNode.Parent;

			_builder.EditPanel.EditElement(_elementEditor,
										   _elementNode.GetNodePath(),
										   _elementNode.ConfigId,
										   _elementNode.DocumentId,
										   _elementNode.ElementType,
										   _elementNode.ElementId,
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