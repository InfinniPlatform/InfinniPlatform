using System.IO;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Settings;

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
            dynamic connectionSettings = new DynamicWrapper();
			connectionSettings.Path = Path.GetFullPath(AppSettings.GetValue("ContentDirectory", "..\\Assemblies\\content"));

            _builder.EditPanel.EditElement(_elementEditor,
                _elementNode.GetNodePath(),
                _elementNode.ConfigId,
                _elementNode.DocumentId,
                _elementNode.Version,
                _elementNode.ElementType,
                _elementNode.ElementId,
                (object) connectionSettings,
                () => Reconnect(connectionSettings));
        }

	    void Reconnect(dynamic connectionSettings)
	    {
		    _builder.EditPanel.CloseAll();

		    _elementNode.ElementName = connectionSettings.Path;
		    _elementNode.RefreshCommand.TryExecute(true);
	    }
    }
}