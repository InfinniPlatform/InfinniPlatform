using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Core.SearchOptions.Builders;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Register;

namespace InfinniPlatform.SystemConfig.ActionUnits.Registers
{
    /// <summary>
    /// Получение значений ресурсов в указанном диапазоне дат для регистра
    /// </summary>
    public sealed class ActionUnitGetRegisterValuesBetweenDates
    {
        public ActionUnitGetRegisterValuesBetweenDates(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public void Action(IApplyResultContext target)
        {
            var startDate = target.Item.FromDate;
            var stopDate = target.Item.ToDate;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();
            var specifiedDimensions = target.Item.Dimensions;

            var registerObject =
                target.Context.GetComponent<IMetadataComponent>()
                      .GetMetadataList(configurationId, registerId, MetadataType.Register)
                      .FirstOrDefault();

            if (registerObject == null)
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = $"Register '{registerId}' not found.";
                return;
            }

            IEnumerable<dynamic> dimensions = specifiedDimensions == null
                ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                : AggregationUtils.BuildDimensionsFromProperties(
                    specifiedDimensions.ToArray());

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

            resultFilter.AddRange(FilterBuilder.DateRangeCondition(RegisterConstants.DocumentDateProperty, startDate,
                stopDate));

            IEnumerable<AggregationType> aggregationTypes;

            if (target.Item.ValueAggregationTypes != null)
            {
                aggregationTypes = new List<AggregationType>();
                foreach (var aggregationType in DynamicWrapperExtensions.ToEnumerable(target.Item.ValueAggregationTypes)
                    )
                {
                    (aggregationTypes as List<AggregationType>).Add((AggregationType)aggregationType);
                }
            }
            else
            {
                // По умолчанию считаем сумму значений
                aggregationTypes = AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount);
            }

            var aggregationResult = _restQueryApi.QueryAggregationRaw(
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
                dimensions.Select(d => (string)d.FieldName).ToArray(),
                values.ToArray(),
                aggregationResult);
        }
    }
}