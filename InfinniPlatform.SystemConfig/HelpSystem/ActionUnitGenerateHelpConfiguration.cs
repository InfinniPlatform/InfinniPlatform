using System;

using InfinniPlatform.Api.SelfDocumentation;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.SystemConfig.HelpSystem
{
    /// <summary>
    /// Пока не используется
    /// </summary>
    public sealed class ActionUnitGenerateHelpConfiguration
    {
        public ActionUnitGenerateHelpConfiguration(IAppConfiguration appConfiguration)
        {
            dynamic helpConfig = appConfiguration.GetSection("help");

            _helpPath = helpConfig?.HelpPath;
        }

        private readonly string _helpPath;

        public void Action(IApplyContext target)
        {
            try
            {
                DocumentationKeeper keeper = DocumentationKeeperBuilder.Build(_helpPath, target.Item.AssemblyPath, new HtmlDocumentationFormatter());
                keeper.SaveHelp(target.Configuration);
                target.ValidationMessage = "Help info successfully created";
                target.IsValid = true;
            }
            catch (Exception e)
            {
                target.ValidationMessage = string.Format("Fail to generate help info about configurations. Error: {0}", e.Message);
                target.IsValid = false;
            }
        }
    }
}