using System;
using System.Collections.Generic;

using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Api.Metadata.MetadataContainers;
using InfinniPlatform.Api.Properties;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories
{
	/// <summary>
	/// Фабрика менеджеров для работы с метаданными документов.
	/// </summary>
	public class ManagerFactoryDocument : IManagerFactoryDocument
	{
		private readonly string _configId;
		private readonly string _documentId;
		private readonly string _documentUid;
		private readonly Dictionary<string, MetadataManagerElement> _managers = new Dictionary<string, MetadataManagerElement>();

		/// <summary>
		/// Публичный конструктор.
		/// </summary>
		/// <param name="configId">Идентификатор конфигурации (например "Integration").</param>
		/// <param name="documentId">Идентификатор документа (например "Patient").</param>
		public ManagerFactoryDocument(string configId, string documentId)
		{
			if (string.IsNullOrEmpty(configId))
			{
				throw new ArgumentException(Resources.ConfigurationIdShouldNotBeEmpty);
			}

			if (string.IsNullOrEmpty(documentId))
			{
				throw new ArgumentException(Resources.DocumentIdShouldNotBeEmpty);
			}

			// внутренний идентификатор (GUid) используемый для однозначного определения элемента на уровне фабрики метаданных
			_documentUid = new ManagerIdentifiersStandard().GetDocumentUid(configId, documentId);

			if (_documentUid == null)
			{
				throw new ArgumentException(string.Format(Resources.DocumentMetadataNotFound, documentId));
			}

			_configId = configId;
			_documentId = documentId;

			_managers.Add(MetadataType.View, BuildViewManager());
			_managers.Add(MetadataType.PrintView, BuildPrintViewManager());
			_managers.Add(MetadataType.Service, BuildServiceManager());
			_managers.Add(MetadataType.Process, BuildProcessManager());
			_managers.Add(MetadataType.Scenario, BuildScenarioManager());
			_managers.Add(MetadataType.Generator, BuildGeneratorManager());
			_managers.Add(MetadataType.ValidationError, BuildValidationErrorsManager());
			_managers.Add(MetadataType.ValidationWarning, BuildValidationWarningsManager());
			_managers.Add(MetadataType.Status, BuildStatusManager());
		}


		// Readers

		public IDataReader BuildViewMetadataReader()
		{
			return BuildMetadataReader(new MetadataContainerView());
		}

		public IDataReader BuildPrintViewMetadataReader()
		{
			return BuildMetadataReader(new MetadataContainerPrintView());
		}

		public IDataReader BuildServiceMetadataReader()
		{
			return BuildMetadataReader(new MetadataContainerService());
		}

		public IDataReader BuildProcessMetadataReader()
		{
			return BuildMetadataReader(new MetadataContainerProcess());
		}

		public IDataReader BuildScenarioMetadataReader()
		{
			return BuildMetadataReader(new MetadataContainerScenario());
		}

		public IDataReader BuildGeneratorMetadataReader()
		{
			return BuildMetadataReader(new MetadataContainerGenerator());
		}

		public IDataReader BuildValidationErrorsMetadataReader()
		{
			return BuildMetadataReader(new MetadataContainerValidationErrors());
		}

		public IDataReader BuildValidationWarningsMetadataReader()
		{
			return BuildMetadataReader(new MetadataContainerValidationWarnings());
		}

		public IDataReader BuildStatusMetadataReader()
		{
			return BuildMetadataReader(new MetadataContainerStatus());
		}

		public IDataReader BuildMetadataReaderByType(string metadataType)
		{
			switch (metadataType)
			{
				case MetadataType.View:
					return BuildViewMetadataReader();
				case MetadataType.PrintView:
					return BuildPrintViewMetadataReader();
				case MetadataType.Service:
					return BuildServiceMetadataReader();
				case MetadataType.Process:
					return BuildProcessMetadataReader();
				case MetadataType.Scenario:
					return BuildScenarioMetadataReader();
				case MetadataType.Generator:
					return BuildGeneratorMetadataReader();
				case MetadataType.ValidationError:
					return BuildValidationErrorsMetadataReader();
				case MetadataType.ValidationWarning:
					return BuildValidationWarningsMetadataReader();
				case MetadataType.Status:
					return BuildStatusMetadataReader();
			}

			return null;
		}

		private IDataReader BuildMetadataReader(IMetadataContainerInfo metadataContainer)
		{
			return new MetadataReaderDocumentElement(_configId, _documentId, metadataContainer);
		}


		// Managers

		public MetadataManagerElement BuildViewManager()
		{
			return BuildMetadataManager(new MetadataContainerView(), MetadataType.View);
		}

		public MetadataManagerElement BuildPrintViewManager()
		{
			return BuildMetadataManager(new MetadataContainerPrintView(), MetadataType.PrintView);
		}

		public MetadataManagerElement BuildGeneratorManager()
		{
			return BuildMetadataManager(new MetadataContainerGenerator(), MetadataType.Generator);
		}

		public MetadataManagerElement BuildScenarioManager()
		{
			return BuildMetadataManager(new MetadataContainerScenario(), MetadataType.Scenario);
		}

		public MetadataManagerElement BuildProcessManager()
		{
			return BuildMetadataManager(new MetadataContainerProcess(), MetadataType.Process);
		}

		public MetadataManagerElement BuildServiceManager()
		{
			return BuildMetadataManager(new MetadataContainerService(), MetadataType.Service);
		}

		public MetadataManagerElement BuildValidationErrorsManager()
		{
			return BuildMetadataManager(new MetadataContainerValidationErrors(), MetadataType.ValidationError);
		}

		public MetadataManagerElement BuildValidationWarningsManager()
		{
			return BuildMetadataManager(new MetadataContainerValidationWarnings(), MetadataType.ValidationWarning);
		}

		public MetadataManagerElement BuildStatusManager()
		{
			return BuildMetadataManager(new MetadataContainerStatus(), MetadataType.Status);
		}

		public MetadataManagerElement BuildManagerByType(string metadataType)
		{
			MetadataManagerElement metadataManager;

			_managers.TryGetValue(metadataType, out metadataManager);

			return metadataManager;
		}

		private MetadataManagerElement BuildMetadataManager(IMetadataContainerInfo metadataContainer, string metadataType)
		{
			var metadataReader = BuildMetadataReader(metadataContainer);
			var metadataCacheRefresher = new MetadataCacheRefresher(_configId, _documentId, metadataType);
			return new MetadataManagerElement(_documentUid, metadataCacheRefresher, metadataReader, metadataContainer, MetadataType.Document);
		}
	}
}