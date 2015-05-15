using System;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.SelfDocumentation;

namespace InfinniPlatform.SystemConfig.HelpSystem
{
    /// <summary>
    ///   Пока не используется
    /// </summary>
    public sealed class ActionUnitGenerateHelpConfiguration
    {
        public void Action(IApplyContext target)
        {
            try
            {
                var keeper = DocumentationKeeperBuilder.Build(target.Item.AssemblyPath, new HtmlDocumentationFormatter());
                keeper.SaveHelp(target.Configuration);
                target.ValidationMessage = "Help info successfully created";
                target.IsValid = true;
            }
            catch (Exception e)
            {
                target.ValidationMessage = string.Format("Fail to generate help info about configurations. Error: {0}",e.Message);
                target.IsValid = false;
            }
        }
    }
}
