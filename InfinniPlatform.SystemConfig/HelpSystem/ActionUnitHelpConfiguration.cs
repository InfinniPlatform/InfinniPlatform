using InfinniPlatform.Api.SelfDocumentation;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.SystemConfig.HelpSystem
{
    public sealed class ActionUnitHelpConfiguration
    {
        public ActionUnitHelpConfiguration(IAppConfiguration appConfiguration)
        {
            dynamic helpConfig = appConfiguration.GetSection("help");

            _helpPath = helpConfig?.HelpPath;
        }


        private readonly string _helpPath;


        public void Action(IApplyResultContext target)
        {
            target.Result = DocumentationKeeper.ReadHelpFromFile(_helpPath, target.Item);
        }
    }
}