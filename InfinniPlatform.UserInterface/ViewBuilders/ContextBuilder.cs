using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.UserInterface.ViewBuilders
{
	sealed class ContextBuilder
	{
		static ContextBuilder()
		{
			GlobalContext = new DynamicWrapper();
			GlobalContext.Clipboard = Clipboard.Instance;
		}


		private static readonly dynamic GlobalContext;


		public static dynamic Build()
		{
			dynamic context = new DynamicWrapper();
			context.Global = GlobalContext;
			return context;
		}
	}
}