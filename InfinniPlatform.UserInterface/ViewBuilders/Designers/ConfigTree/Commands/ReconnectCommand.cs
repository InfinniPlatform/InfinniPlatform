using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
    internal sealed class ReconnectCommand : CommandBase<object>
    {
	    private readonly ConfigElementNodeBuilder _builder;
        private readonly string _elementEditor;
        private readonly ConfigElementNode _elementNode;

        public ReconnectCommand(ConfigElementNodeBuilder builder, ConfigElementNode elementNode, string elementEditor)
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
            dynamic metadataPath = new DynamicWrapper();
			metadataPath.Path = _elementNode.ElementName;
			
            _builder.EditPanel.EditElement(_elementEditor,
                _elementNode.GetNodePath(),
                _elementNode.ConfigId,
                _elementNode.DocumentId,
                _elementNode.Version,
                _elementNode.ElementType,
                _elementNode.ElementId,
                (object) metadataPath,
                () => Reconnect(metadataPath));
        }

	    void Reconnect(dynamic metadataPath)
	    {
		    _builder.EditPanel.CloseAll();

		    _elementNode.ElementName = metadataPath.Path;
		    PackageMetadataLoader.UpdateCache(metadataPath.Path);
			_elementNode.RefreshCommand.TryExecute(true);
	    }
    }
}