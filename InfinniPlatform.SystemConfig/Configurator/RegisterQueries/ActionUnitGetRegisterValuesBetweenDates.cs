using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Api.SearchOptions.Converters;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Register;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator.RegisterQueries
{
    /// <summary>
    ///     Получение значений ресурсов в указанном диапазоне дат для регистра
    /// </summary>
    public sealed class ActionUnitGetRegisterValuesBetweenDates
    {
        public void Action(IApplyResultContext target)
        {
            var startDate = target.Item.FromDate;
            var stopDate = target.Item.ToDate;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();
            var specifiedDimensions = target.Item.Dimensions;

            var registerObject =
                target.Context.GetComponent<IMetadataComponent>()
                      .GetMetadataList(null, configurationId, registerId, MetadataType.Register)
                      .FirstOrDefault();

            if (registerObject == null)
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = string.Format(Resources.RegisterNotFound, registerId);
                return;
            }

            IEnumerable<dynamic> dimensions = specifiedDimensions == null
                                                  ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                                                  : AggregationUtils.BuildDimensionsFromProperties(
                                                      specifiedDimensions.ToArray());

            var valueProperties = target.Item.ValueProperties ??
                                  AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            int valuePropertiesCount = 0;
            var values = new List<string>();

            foreach (var valueProperty in valueProperties)
            {
                valuePropertiesCount++;
                values.Add(valueProperty.ToString());
            }

            var resultFilter = new List<object>();
            IEnumerable<object> filter = DynamicWrapperExtensions.ToEnumerable(target.Item.Filter);

            if (filter != null)
            {
                resultFilter.AddRange(filter);
            }

            resultFilter.AddRange(FilterBuilder.DateRangeCondition(RegisterConstants.DocumentDateProperty, startDate,
                                                                   stopDate));

            IEnumerable<AggregationType> aggregationTypes;

            if (target.Item.ValueAggregationTypes != null)
            {
                aggregationTypes = new List<AggregationType>();
                foreach (var aggregationType in DynamicWrapperExtensions.ToEnumerable(target.Item.ValueAggregationTypes)
                    )
                {
                    (aggregationTypes as List<AggregationType>).Add((AggregationType) aggregationType);
                }
            }
            else
            {
                // По умолчанию считаем сумму значений
                aggregationTypes = AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount);
            }

            IEnumerable<dynamic> aggregationResult = RestQueryApi.QueryAggregationRaw(
                "SystemConfig",
                "metadata",
                "aggregate",
                configurationId,
                RegisterConstants.RegisterNamePrefix + registerId,
                resultFilter,
                dimensions,
                aggregationTypes,
                values,
                0,
                10000)
                                                                 .ToDynamicList();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            target.Result = AggregationUtils.ProcessBuckets(
                dimensions.Select(d => (string) d.FieldName).ToArray(),
                values.ToArray(),
                aggregationResult);
        }
    }
}