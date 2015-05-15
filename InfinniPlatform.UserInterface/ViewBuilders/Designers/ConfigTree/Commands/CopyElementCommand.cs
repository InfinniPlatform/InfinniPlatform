namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
	sealed class CopyElementCommand : CommandBase<object>
	{
		private readonly ConfigElementNode _elementNode;

		public CopyElementCommand(ConfigElementNode elementNode)
		{
			_elementNode = elementNode;
		}

		public override bool CanExecute(object parameter)
		{
			return (_elementNode != null)
				   && !string.IsNullOrEmpty(_elementNode.ElementId)
				   && !string.IsNullOrEmpty(_elementNode.ElementType);
		}

		public override void Execute(object parameter)
		{
			CommandHelper.Clipboard = _elementNode;
		}
	}
}