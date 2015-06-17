using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator.RegisterQueries
{
    /// <summary>
    /// Получение значений ресурсов на указанную дату
    /// </summary>
    public sealed class ActionUnitGetRegisterValuesByDate
    {
        public void Action(IApplyResultContext target)
        {
            var aggregationDate = target.Item.Date;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();
            var specifiedDimensions = target.Item.Dimensions;

            var registerObject = target.Context.GetComponent<IMetadataComponent>(target.Version).GetMetadataList(target.Version, configurationId, registerId,MetadataType.Register).FirstOrDefault();

            if (registerObject == null)
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = string.Format(Resources.RegisterNotFound, registerId);
                return;
            }
            
            // Сначала необходимо извлечь значения из регистра итогов
            var closestDate =
                RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "GetClosestDateTimeOfTotalCalculation", null, new
                {
                    Configuration = configurationId,
                    Register = registerId,
                    Date = aggregationDate
                }, target.Version).ToDynamic();

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(aggregationDate));

            IEnumerable<dynamic> aggregatedTotals = null;

            if (closestDate != null)
            {
                aggregatedTotals = new DocumentApi(target.Version).GetDocument(
                    configurationId,
                    RegisterConstants.RegisterTotalNamePrefix + registerId,
                    f => f.AddCriteria(
                        c => c.Property(RegisterConstants.DocumentDateProperty).IsEquals(closestDate.Date)), 0, 10000);

                filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsMoreThan(closestDate.Date));
            }

            IEnumerable<dynamic> dimensions = specifiedDimensions == null ?
                AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject) :
                AggregationUtils.BuildDimensionsFromProperties(specifiedDimensions);

            var valueProperties = target.Item.ValueProperties ??
                AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = 0;
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

            resultFilter.AddRange(filetrBuilder.GetFilter());

            IEnumerable<AggregationType> aggregationTypes;

            if (target.Item.ValueAggregationTypes != null)
            {
                aggregationTypes = new List<AggregationType>();
                foreach (var aggregationType in DynamicWrapperExtensions.ToEnumerable(target.Item.ValueAggregationTypes))
                {
                    (aggregationTypes as List<AggregationType>).Add((AggregationType)aggregationType);
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

            var dimensionNames = dimensions.Select(d => (string) d.FieldName).ToArray();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            var denormalizedResult = AggregationUtils.ProcessBuckets(dimensionNames, values.ToArray(), aggregationResult);

            if (aggregatedTotals != null)
            {
                target.Result = AggregationUtils.MergeAggregaionResults(dimensionNames, valueProperties,
                    denormalizedResult, aggregatedTotals);
            }
            else
            {
                target.Result = denormalizedResult;
            }
        }
    }
}
