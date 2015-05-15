using System;

using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.ValidationBuilders;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
	/// <summary>
	/// API для действий с генераторами.
	/// Необходим для упрощения работы с генераторами, так как действия с метаданными генераторов 
	/// охватывают несколько типов метаданных элементов (сценарии, бизнес-процессы, сервисы)
	/// </summary>
	public sealed class GeneratorBroker
	{
		public GeneratorBroker(string configurationId, string documentId)
		{
			_configurationId = configurationId;
			_documentId = documentId;
		}


		private readonly string _configurationId;
		private readonly string _documentId;


		public void CreateGenerator(dynamic generatorObject)
		{
			generatorObject = DynamicWrapperExtensions.ToDynamic(generatorObject);

			var validator = ValidationBuilder.ForObject(builder => builder.And(rules => rules
				.IsNotNullOrEmpty("GeneratorName", Resources.GeneratorNameShouldNotBeEmpty)
				.IsNotNullOrEmpty("ActionUnit", Resources.ActionUnitShouldNotBeEmpty)
				.IsNotNullOrEmpty("MetadataType", Resources.MetadataTypeForGeneratorShouldNotBeEmpty)));

			var validationResult = new ValidationResult();

			if (validator.Validate((object)generatorObject, validationResult))
			{
				

				var eventObject = new
								  {
									  GeneratorName = generatorObject.GeneratorName,
									  ActionUnit = generatorObject.ActionUnit,
									  Configuration = _configurationId,
									  Metadata = _documentId,
									  MetadataType = generatorObject.MetadataType,
									  ContextTypeKind = ContextTypeKind.ApplyMove
								  };

				RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "creategenerator",null, eventObject);
			}
			else
			{
				throw new ArgumentException(validationResult.ToDynamic().ToString());
			}
		}

		public void DeleteGenerator(string generatorName)
		{
			var body = new
					   {
						   Configuration = _configurationId,
						   Metadata = _documentId,
						   GeneratorName = generatorName
					   };

			RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "deletegenerator", null, body);
		}
	}
}