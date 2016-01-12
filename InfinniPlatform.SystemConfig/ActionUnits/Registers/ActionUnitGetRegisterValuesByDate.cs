using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Registers;
using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Core.SearchOptions.Builders;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.SystemConfig.ActionUnits.Registers
{
    /// <summary>
    /// Получение значений ресурсов на указанную дату
    /// </summary>
    public sealed class ActionUnitGetRegisterValuesByDate
    {
        public ActionUnitGetRegisterValuesByDate(RestQueryApi restQueryApi, DocumentApi documentApi, IMetadataComponent metadataComponent)
        {
            _restQueryApi = restQueryApi;
            _documentApi = documentApi;
            _metadataComponent = metadataComponent;
        }

        private readonly DocumentApi _documentApi;
        private readonly IMetadataComponent _metadataComponent;
        private readonly RestQueryApi _restQueryApi;

        public void Action(IApplyResultContext target)
        {
            var aggregationDate = target.Item.Date;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();
            var specifiedDimensions = target.Item.Dimensions;

            var registerObject = _metadataComponent
                .GetMetadataList(configurationId, registerId, MetadataType.Register)
                .FirstOrDefault();

            if (registerObject == null)
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = $"Register '{registerId} not found'";
                return;
            }

            // Сначала необходимо извлечь значения из регистра итогов
            dynamic closestDate =
                _restQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "GetClosestDateTimeOfTotalCalculation", null,
                    new
                    {
                        Configuration = configurationId,
                        Register = registerId,
                        Date = aggregationDate
                    }).ToDynamic();

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(
                c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(aggregationDate));

            IEnumerable<dynamic> aggregatedTotals = null;

            if (closestDate != null)
            {
                aggregatedTotals = _documentApi.GetDocument(
                    configurationId,
                    RegisterConstants.RegisterTotalNamePrefix + registerId,
                    f => f.AddCriteria(
                        c => c.Property(RegisterConstants.DocumentDateProperty).IsEquals(closestDate.Date)), 0, 10000);

                filetrBuilder.AddCriteria(
                    c => c.Property(RegisterConstants.DocumentDateProperty).IsMoreThan(closestDate.Date));
            }

            IEnumerable<dynamic> dimensions = specifiedDimensions == null
                ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                : AggregationUtils.BuildDimensionsFromProperties(specifiedDimensions);

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

            var dimensionNames = dimensions.Select(d => (string)d.FieldName).ToArray();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            var denormalizedResult = AggregationUtils.ProcessBuckets(dimensionNames, values.ToArray(),
                aggregationResult);

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