using InfinniPlatform.Api.SelfDocumentation;
using InfinniPlatform.Sdk.Application.Contracts;

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