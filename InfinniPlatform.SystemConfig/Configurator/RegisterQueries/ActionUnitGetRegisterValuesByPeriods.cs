using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Index;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Register;
using InfinniPlatform.SystemConfig.Properties;

namespace InfinniPlatform.SystemConfig.Configurator.RegisterQueries
{
    /// <summary>
    ///     Получение значений ресурсов в указанном диапазоне дат c разбиением на периоды
    /// </summary>
    public sealed class ActionUnitGetRegisterValuesByPeriods
    {
        public void Action(IApplyResultContext target)
        {
            var startDate = target.Item.FromDate;
            var stopDate = target.Item.ToDate;
            string interval = target.Item.Interval;
            string timezone = target.Item.TimeZone;
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
                target.Result.ValidationMessage = string.Format(Resources.RegisterNotFound, registerId);
                return;
            }

            // В качестве интервалов могут быть указаны следующие значения:
            // year, quarter, month, week, day, hour, minute, second

            if (!CheckInterval(interval))
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = string.Format(Resources.SpecifiedIntervalIsInvalidSupportedIntervals,
                                                                interval);
                return;
            }

            if (string.IsNullOrEmpty(timezone))
            {
                double hours = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalHours;
                timezone = hours > 0 ? "+" + hours.ToString("00") + ":00" : hours.ToString("00") + ":00";
            }

            if (!CheckTimezone(timezone))
            {
                target.Result.IsValid = false;
                target.Result.ValidationMessage = string.Format(Resources.SpecifiedTimezoneIsInvalid, timezone);
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

            IEnumerable<dynamic> aggregationResult = RestQueryApi.QueryAggregationRaw(
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
                dimensions.Select(d => (string) d.FieldName).ToArray(),
                valueProperties.ToArray(),
                aggregationResult);
        }

        private static bool CheckInterval(string interval)
        {
            var validIntervals = new[] {"year", "quarter", "month", "week", "day", "hour", "minute", "second"};

            return validIntervals.Contains(interval.ToLowerInvariant());
        }

        private static bool CheckTimezone(string timezone)
        {
            // Временная зона задаётся в виде "+05:00"
            return (Regex.IsMatch(timezone, "^[+-]?[0-9]*:00$"));
        }
    }
}