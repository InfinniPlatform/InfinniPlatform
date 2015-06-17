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
		private static readonly Dictionary<string, Func<string, string, string, IMetadataService>> MetadataServices
			= new Dictionary<string, Func<string, string, string, IMetadataService>>
			  {
				  { MetadataType.Configuration, (version, configId, documentId) => new ConfigurationMetadataService(version) },
				  { MetadataType.Menu, (version, configId, documentId) => new MenuMetadataService(version, configId) },
				  { MetadataType.Document, (version, configId, documentId) => new DocumentMetadataService(version, configId) },
				  { MetadataType.Assembly, (version, configId, documentId) => new AssemblyMetadataService(version, configId) },
				  { MetadataType.Register, (version, configId, documentId) => new RegisterMetadataService(version, configId) },
				  { MetadataType.Report, (version, configId, documentId) => new ReportMetadataService(version, configId) },
				  { MetadataType.View, (version, configId, documentId) => new ViewMetadataService(version, configId, documentId) },
				  { MetadataType.PrintView, (version, configId, documentId) => new PrintViewMetadataService(version, configId, documentId) },
				  { MetadataType.Service, (version, configId, documentId) => new ServiceMetadataService(version, configId, documentId) },
				  { MetadataType.Process, (version, configId, documentId) => new ProcessMetadataService(version, configId, documentId) },
				  { MetadataType.Scenario, (version, configId, documentId) => new ScenarioMetadataService(version, configId, documentId) },
				  { MetadataType.Generator, (version, configId, documentId) => new GeneratorMetadataService(version, configId, documentId) },
				  { MetadataType.ValidationError, (version, configId, documentId) => new ValidationErrorMetadataService(version, configId,documentId)},
				  { MetadataType.ValidationWarning, (version, configId, documentId) => new ValidationWarningMetadataService(version, configId,documentId)},
                  { MetadataType.Status, (version, configId, documentId) => new StatusMetadataService(version, configId,documentId)}
			  };


		public MetadataProvider(string metadataType)
		{
			_metadataType = metadataType;
		}


		private readonly string _metadataType;


		private string _configId;

	    private string _versionId;

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

	    public string GetVersion()
	    {
	        return _versionId;
	    }

	    public void SetVersion(string value)
	    {
	        if (Equals(_versionId, value) == false)
	        {
	            _versionId = value;

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
						Func<string, string, string, IMetadataService> metadataServiceFunc;

						if (MetadataServices.TryGetValue(_metadataType, out metadataServiceFunc) == false)
						{
							throw new NotSupportedException(_metadataType);
						}

						_metadataService = metadataServiceFunc(_versionId, _configId, _documentId);
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