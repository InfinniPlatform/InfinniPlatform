using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.SelfDocumentation;

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
