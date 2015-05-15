using System;
using System.Collections;
using System.Collections.Generic;

using InfinniPlatform.Api.Metadata;
using InfinniPlatform.ReportDesigner.Services;
using InfinniPlatform.UserInterface.Services.Metadata;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataProviders
{
	/// <summary>
	/// Провайдер данных для метаданных.
	/// </summary>
	public sealed class MetadataProvider : IDataProvider
	{
		private static readonly Dictionary<string, Func<string, string, IMetadataService>> MetadataServices
			= new Dictionary<string, Func<string, string, IMetadataService>>
			  {
				  { MetadataType.Configuration, (configId, documentId) => ConfigurationMetadataService.Instance },
				  { MetadataType.Menu, (configId, documentId) => new MenuMetadataService(configId) },
				  { MetadataType.Document, (configId, documentId) => new DocumentMetadataService(configId) },
				  { MetadataType.Assembly, (configId, documentId) => new AssemblyMetadataService(configId) },
				  { MetadataType.Register, (configId, documentId) => new RegisterMetadataService(configId) },
				  { MetadataType.Report, (configId, documentId) => new ReportMetadataService(configId) },
				  { MetadataType.View, (configId, documentId) => new ViewMetadataService(configId, documentId) },
				  { MetadataType.PrintView, (configId, documentId) => new PrintViewMetadataService(configId, documentId) },
				  { MetadataType.Service, (configId, documentId) => new ServiceMetadataService(configId, documentId) },
				  { MetadataType.Process, (configId, documentId) => new ProcessMetadataService(configId, documentId) },
				  { MetadataType.Scenario, (configId, documentId) => new ScenarioMetadataService(configId, documentId) },
				  { MetadataType.Generator, (configId, documentId) => new GeneratorMetadataService(configId, documentId) },
				  { MetadataType.ValidationError, (configId, documentId) => new ValidationErrorMetadataService(configId,documentId)},
				  { MetadataType.ValidationWarning, (configId, documentId) => new ValidationWarningMetadataService(configId,documentId)},
                  { MetadataType.Status, (configId, documentId) => new StatusMetadataService(configId,documentId)}
			  };


		public MetadataProvider(string metadataType)
		{
			_metadataType = metadataType;
		}


		private readonly string _metadataType;


		private string _configId;

		public string GetConfigId()
		{
			return _configId;
		}

		public void SetConfigId(string value)
		{
			if (Equals(_configId, value) == false)
			{
				_configId = value;

				ResetMetadataService();
			}
		}


		private string _documentId;

		public string GetDocumentId()
		{
			return _documentId;
		}

		public void SetDocumentId(string value)
		{
			if (Equals(_documentId, value) == false)
			{
				_documentId = value;

				ResetMetadataService();
			}
		}


		private IMetadataService _metadataService;

		private void ResetMetadataService()
		{
			if (_metadataService != null)
			{
				lock (this)
				{
					_metadataService = null;
				}
			}
		}

		private IMetadataService GetMetadataService()
		{
			if (_metadataService == null)
			{
				lock (this)
				{
					if (_metadataService == null)
					{
						Func<string, string, IMetadataService> metadataServiceFunc;

						if (MetadataServices.TryGetValue(_metadataType, out metadataServiceFunc) == false)
						{
							throw new NotSupportedException(_metadataType);
						}

						_metadataService = metadataServiceFunc(_configId, _documentId);
					}
				}
			}

			return _metadataService;
		}


		public object CreateItem()
		{
			return GetMetadataService().CreateItem();
		}

		public void ReplaceItem(object item)
		{
			GetMetadataService().ReplaceItem(item);
		}

		public void DeleteItem(string itemId)
		{
			GetMetadataService().DeleteItem(itemId);
		}

		public object GetItem(string itemId)
		{
			return GetMetadataService().GetItem(itemId);
		}

		public object CloneItem(string itemId)
		{
			return GetMetadataService().CloneItem(itemId);
		}

		public IEnumerable GetItems(IEnumerable criterias, int pageNumber, int pageSize)
		{
			return GetMetadataService().GetItems();
		}
	}
}