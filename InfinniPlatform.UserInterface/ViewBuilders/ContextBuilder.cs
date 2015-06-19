using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.UserInterface.ViewBuilders
{
    internal sealed class ContextBuilder
    {
        private static readonly dynamic GlobalContext;

        static ContextBuilder()
        {
            GlobalContext = new DynamicWrapper();
            GlobalContext.Clipboard = Clipboard.Instance;
        }

        public static dynamic Build()
        {
            dynamic context = new DynamicWrapper();
            context.Global = GlobalContext;
            return context;
        }
    }
}