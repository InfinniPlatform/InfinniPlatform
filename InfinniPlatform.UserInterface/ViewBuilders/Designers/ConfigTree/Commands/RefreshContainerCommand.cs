namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
    internal sealed class RefreshContainerCommand : CommandBase<bool>
    {
        private readonly ConfigElementNode _elementNode;

        public RefreshContainerCommand(ConfigElementNode elementNode)
        {
            _elementNode = elementNode;
        }

        public override bool CanExecute(bool reload)
        {
            return true;
        }

        public override void Execute(bool reload)
        {
            if (reload || !_elementNode.IsLoaded)
            {
                _elementNode.IsLoaded = true;
            }
        }
    }
}