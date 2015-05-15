using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataBindings
{
	sealed class ObjectBindingBuilder : IObjectBuilder
	{
		public object Build(ObjectBuilderContext context, View parent, dynamic metadata)
		{
			return new ObjectBinding(parent, metadata.Value);
		}
	}
}