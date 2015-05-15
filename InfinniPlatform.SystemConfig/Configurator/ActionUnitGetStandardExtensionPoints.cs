using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetStandardExtensionPoints
    {
        public void Action(ISearchContext context)
        {
            context.SearchResult = new string[]
                {
                    "Action",
                    "OnSuccess",
                    "OnFail",
                    "Validation"
                };
        }
    }
}
