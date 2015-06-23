using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetStandardExtensionPoints
    {
        public void Action(ISearchContext context)
        {
            context.SearchResult = new[]
                {
                    "Action",
                    "OnSuccess",
                    "OnFail",
                    "Validation"
                };
        }
    }
}