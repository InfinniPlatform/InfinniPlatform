﻿using System;
using System.IO;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Update.Properties;

namespace InfinniPlatform.Update.ActionUnits
{
	public sealed class ActionUnitSaveAssemblyVersion
	{
		public void Action(IApplyContext target)
		{
			// получаем конструктор метаданных конфигураций
			var configBuilder = target.Context.GetComponent<IConfigurationMediatorComponent>(target.Version).ConfigurationBuilder;

			// получаем конфигурацию обновления
			var config = configBuilder.GetConfigurationObject(null, "update");

			string version = target.Version;

			var documentProvider = config.GetDocumentProvider("package","system");

			// публикуем прикладную сборку

			var contentEvent = target.Item.Assembly;

			if (contentEvent != null)
			{
				var blobStorage = target.Context.GetComponent<IBlobStorageComponent>(target.Version).GetBlobStorage();

				byte[] contentData = Convert.FromBase64String(contentEvent);
				var contentName = target.Item.ModuleId;

				if (target.Item.ContentId == null)
				{
					var contentId = Guid.NewGuid();
					blobStorage.SaveBlob(contentId, contentName, contentData);
					target.Item.ContentId = contentId;
				}
				else
				{
					var contentId = new Guid(target.Item.ContentId);
					blobStorage.SaveBlob(contentId, contentName, contentData);
				}

				// добавляем информацию для отладчика

				var pdbEvent = target.Item.PdbFile;

				if (pdbEvent != null)
				{
					byte[] pdbData = Convert.FromBase64String(pdbEvent);
					var pdbName = Path.GetFileNameWithoutExtension(target.Item.ModuleId) + ".pdb";

					if (target.Item.PdbId == null)
					{
						var pdbId = Guid.NewGuid();
						blobStorage.SaveBlob(pdbId, pdbName, pdbData);
						target.Item.PdbId = pdbId;
					}
					else
					{
						var pdbId = new Guid(target.Item.PdbId);
						blobStorage.SaveBlob(pdbId, pdbName, pdbData);
					}
				}
			}

			target.Item.Assembly = null;
			target.Item.PdbFile = null;

			dynamic criteria = new DynamicWrapper();
			criteria.Property = "ConfigurationName";
			criteria.Value = target.Item.ConfigurationName;
			criteria.CriteriaType = CriteriaType.IsEquals;

			dynamic criteria1 = new DynamicWrapper();
			criteria1.Property = "Version";
			criteria1.Value = version;
			criteria1.CriteriaType = CriteriaType.IsEquals;

			dynamic criteria2 = new DynamicWrapper();
			criteria2.Property = "ModuleId";
			criteria2.Value = target.Item.ModuleId;
			criteria2.CriteriaType = CriteriaType.IsEquals;

			// если пакет с таким же идентификатором версии не существует, создается новый пакет и, соответственно, новая версия системы

			var packagesExisting = documentProvider.GetDocument(new[] { criteria, criteria1, criteria2 }, 0, 1);

			if (packagesExisting != null && packagesExisting.Count > 0)
			{
				target.Item.Id = packagesExisting[0].Id;
			}

			// необходимо хранить все версии пакетов для развертывания всех существующих версий конфигураций
			documentProvider.SetDocument(target.Item);
		}
	}
}