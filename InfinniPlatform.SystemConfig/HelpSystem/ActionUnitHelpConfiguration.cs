using InfinniPlatform.Api.SelfDocumentation;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.HelpSystem
{
    public sealed class ActionUnitHelpConfiguration
    {
        public void Action(IApplyResultContext target)
        {
            target.Result = DocumentationKeeper.ReadHelpFromFile(target.Item);
        }
    }
}