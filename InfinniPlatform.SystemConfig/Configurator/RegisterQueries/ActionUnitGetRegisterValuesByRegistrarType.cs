using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator.RegisterQueries
{
    /// <summary>
    ///     Получение значений ресурсов по типу документа-регистратора
    /// </summary>
    public sealed class ActionUnitGetRegisterValuesByRegistrarType
    {
        public void Action(IApplyResultContext target)
        {
            var registrarType = target.Item.RegistrarType;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();
            var specifiedDimensions = target.Item.Dimensions;

            var registerObject =
                target.Context.GetComponent<IMetadataComponent>(target.Version)
                      .GetMetadataList(target.Version, configurationId, registerId, MetadataType.Register)
                      .FirstOrDefault();

            if (registerObject == null)
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = string.Format(Resources.RegisterNotFound, registerId);
                return;
            }

            IEnumerable<dynamic> dimensions = specifiedDimensions == null
                                                  ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                                                  : AggregationUtils.BuildDimensionsFromProperties(specifiedDimensions);

            var valueProperties = target.Item.ValueProperties ??
                                  AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.RegistrarTypeProperty).IsEquals(registrarType));

            IEnumerable<dynamic> aggregationResult = RestQueryApi.QueryAggregationRaw(
                "SystemConfig",
                "metadata",
                "aggregate",
                configurationId,
                RegisterConstants.RegisterNamePrefix + registerId,
                filetrBuilder.GetFilter(),
                dimensions,
                AggregationUtils.BuildAggregationType(AggregationType.Sum,
                                                      valueProperties is List<string>
                                                          ? valueProperties.Count
                                                          : valueProperties.Count()),
                valueProperties.ToArray(),
                0,
                10000)
                                                                 .ToDynamicList();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            target.Result = AggregationUtils.ProcessBuckets(dimensions.Select(d => (string) d.FieldName).ToArray(),
                                                            valueProperties.ToArray(), aggregationResult);
        }
    }
}