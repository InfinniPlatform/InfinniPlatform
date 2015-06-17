using System;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Update.Properties;

namespace InfinniPlatform.Update.ActionUnits
{
	/// <summary>
	///   Установка JSON конфигурации
	/// </summary>
	sealed class ActionUnitInstallJsonMetadata
	{
		public void Action(IApplyContext target)
		{
			if (string.IsNullOrEmpty(target.Item.ConfigurationId))
			{
				var emptyNameMessage = Resources.EmptyNameMessage;
                target.Context.GetComponent<ILogComponent>(target.Version).GetLog().Error(emptyNameMessage);
				throw new ArgumentException(emptyNameMessage);
			}

			if (string.IsNullOrEmpty(target.Item.MetadataType))
			{
				throw new ArgumentException(Resources.ErrorMetadataTypeNotSpecified);
			}

			if (target.Item.MetadataType == MetadataType.Configuration)
			{
				ManagerFactoryConfiguration.BuildConfigurationManager(target.Version).MergeItem(target.Item.MetadataObject);
			}
			else if (target.Item.MetadataType == MetadataType.Menu)
			{
				var manager = new ManagerFactoryConfiguration(target.Version, target.Item.ConfigurationId).BuildMenuManager();
				manager.MergeItem(target.Item.MetadataObject);
			}
			else if (target.Item.MetadataType == MetadataType.Report)
			{
				var manager = new ManagerFactoryConfiguration(target.Version, target.Item.ConfigurationId).BuildReportManager();
				manager.MergeItem(target.Item.MetadataObject);
			}
			else if (target.Item.MetadataType == MetadataType.Document)
			{
				var manager = new ManagerFactoryConfiguration(target.Version, target.Item.ConfigurationId).BuildDocumentManager();
				manager.MergeItem(target.Item.MetadataObject);				
			}
			else if (target.Item.MetadataType == MetadataType.Assembly)
			{
				var manager = new ManagerFactoryConfiguration(target.Version, target.Item.ConfigurationId).BuildAssemblyManager();
				manager.MergeItem(target.Item.MetadataObject);				
			}
            else if (target.Item.MetadataType == MetadataType.Register)
            {
                var manager = new ManagerFactoryConfiguration(target.Version, target.Item.ConfigurationId).BuildRegisterManager();
                manager.MergeItem(target.Item.MetadataObject);
            }
			else
			{

				var manager = new ManagerFactoryDocument(target.Version, target.Item.ConfigurationId, target.Item.DocumentId).BuildManagerByType(
						target.Item.MetadataType);
				manager.MergeItem(target.Item.MetadataObject);
			}
		}
	}
}
