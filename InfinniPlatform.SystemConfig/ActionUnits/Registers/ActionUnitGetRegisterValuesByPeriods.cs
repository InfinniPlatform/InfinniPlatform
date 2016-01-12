using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Registers;
using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.SystemConfig.ActionUnits.Registers
{
    /// <summary>
    /// Получение значений ресурсов в указанном диапазоне дат c разбиением на периоды
    /// </summary>
    public sealed class ActionUnitGetRegisterValuesByPeriods
    {
        public ActionUnitGetRegisterValuesByPeriods(RestQueryApi restQueryApi, IMetadataComponent metadataComponent)
        {
            _restQueryApi = restQueryApi;
            _metadataComponent = metadataComponent;
        }

        private readonly RestQueryApi _restQueryApi;
        private readonly IMetadataComponent _metadataComponent;

        public void Action(IApplyResultContext target)
        {
            var startDate = target.Item.FromDate;
            var stopDate = target.Item.ToDate;
            string interval = target.Item.Interval;
            string timezone = target.Item.TimeZone;
            string configurationId = target.Item.Configuration.ToString();
            string registerId = target.Item.Register.ToString();
            var specifiedDimensions = target.Item.Dimensions;

            var registerObject = _metadataComponent
                      .GetMetadataList(configurationId, registerId, MetadataType.Register)
                      .FirstOrDefault();

            if (registerObject == null)
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = $"Register '{registerId}' not found.";
                return;
            }

            // В качестве интервалов могут быть указаны следующие значения:
            // year, quarter, month, week, day, hour, minute, second

            if (!CheckInterval(interval))
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = $"Specified interval '{interval}' is invalid. Supported intervals: year, quarter, month, week, day, hour, minute, second.";
                return;
            }

            if (string.IsNullOrEmpty(timezone))
            {
                var hours = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalHours;
                timezone = hours > 0 ? "+" + hours.ToString("00") + ":00" : hours.ToString("00") + ":00";
            }

            if (!CheckTimezone(timezone))
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = $"Specified timezone {timezone} is invalid. Valid timezone example: '+05:00'.";
                return;
            }

            var dimensions = new List<dynamic>
                             {
                                 new
                                 {
                                     Label = RegisterConstants.DocumentDateProperty + "_datehistogram",
                                     FieldName = RegisterConstants.DocumentDateProperty,
                                     DimensionType = DimensionType.DateHistogram,
                                     Interval = interval,
                                     TimeZone = timezone
                                 }.ToDynamic()
                             };

            dimensions.AddRange(specifiedDimensions == null
                ? AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject)
                : AggregationUtils.BuildDimensionsFromProperties(specifiedDimensions));

            var valueProperties = target.Item.ValueProperties ??
                                  AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var resultFilter = new List<object>();
            IEnumerable<object> filter = DynamicWrapperExtensions.ToEnumerable(target.Item.Filter);

            if (filter != null)
            {
                resultFilter.AddRange(filter);
            }

            resultFilter.AddRange(FilterBuilder.DateRangeCondition(RegisterConstants.DocumentDateProperty, startDate,
                stopDate));

            IEnumerable<dynamic> aggregationResult = _restQueryApi.QueryAggregationRaw(
                "SystemConfig",
                "metadata",
                "aggregate",
                configurationId,
                RegisterConstants.RegisterNamePrefix + registerId,
                resultFilter,
                dimensions,
                AggregationUtils.BuildAggregationType(AggregationType.Sum,
                    valueProperties is List<string>
                        ? valueProperties.Count
                        : valueProperties.Count()),
                valueProperties,
                0,
                10000)
                                                                  .ToDynamicList();

            // Выполняем обработку результата агрегации, чтобы представить полученные данные в табличном виде
            target.Result = AggregationUtils.ProcessBuckets(
                dimensions.Select(d => (string)d.FieldName).ToArray(),
                valueProperties.ToArray(),
                aggregationResult);
        }

        private static bool CheckInterval(string interval)
        {
            var validIntervals = new[] { "year", "quarter", "month", "week", "day", "hour", "minute", "second" };

            return validIntervals.Contains(interval.ToLowerInvariant());
        }

        private static bool CheckTimezone(string timezone)
        {
            // Временная зона задаётся в виде "+05:00"
            return (Regex.IsMatch(timezone, "^[+-]?[0-9]*:00$"));
        }
    }
}