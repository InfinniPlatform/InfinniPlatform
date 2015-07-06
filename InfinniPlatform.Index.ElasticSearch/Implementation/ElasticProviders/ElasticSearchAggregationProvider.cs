﻿
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion;
using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeSelectors;
using InfinniPlatform.Sdk.Environment.Index;
using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using InfinniPlatform.Sdk.Dynamic;
using IFilter = InfinniPlatform.Sdk.Environment.Index.IFilter;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    public sealed class ElasticSearchAggregationProvider : IAggregationProvider
    {
	    private readonly string _routing;
	    private readonly string _indexName;
        private readonly IEnumerable<IndexToTypeAccordance> _typeName;
        private IFilter _filters;
        private readonly ElasticConnection _elasticConnection;

        public ElasticSearchAggregationProvider(string indexName, string typeName, string routing)
        {
	        _routing = routing;
	        _indexName = indexName.ToLowerInvariant();
            _elasticConnection = new ElasticConnection();

            _elasticConnection.ConnectIndex();

            _typeName = _elasticConnection.GetAllTypes(new[] {_indexName}, new[] {typeName.ToLowerInvariant()});
        }

        /// <summary>
        /// Выполнение агрегирующего запроса
        /// </summary>
        /// <param name="dimensions">Срезы OLAP куба. Метод позволяет выполнять различные типы срезов (например, Term, Range, DateRange) </param>
        /// <param name="measureTypes">Типы измерений. Количество типов измерений должено соответствовать указанному количеству measureFieldNames</param>
        /// <param name="measureFieldNames">Имена свойств, по которым необходимо произвести вычисление</param>
        /// <param name="filters">Фильтр для данных</param>
        /// <returns>Результат выполнения агрегации</returns>
        public IEnumerable<AggregationResult> ExecuteAggregation(
            dynamic[] dimensions, 
            AggregationType[] measureTypes, 
            string[] measureFieldNames, 
            IFilter filters = null)
        {
            Func<AggregationDescriptor<dynamic>, AggregationDescriptor<dynamic>> activeAggregation = null;

            if (measureFieldNames == null || measureFieldNames.Length == 0)
            {
                activeAggregation =
                    s => s.ValueCount("ValueCount0", f => f.Field(ElasticConstants.IndexObjectPath + "Id"));
            }

            for (int measure = 0; measure < measureTypes.Length; measure++)
            {
                if (measureFieldNames != null && 
                    measureFieldNames.Length > 0 && 
                    measureFieldNames[measure] != null)
                {
                    switch (measureTypes[measure])
                    {
                        case AggregationType.Min:
                            var measureSavedMin = measure;
                            activeAggregation += s => s.Min(string.Format("Min{0}", measureSavedMin),
                                f => f.Field(ElasticConstants.IndexObjectPath + measureFieldNames[measureSavedMin]));
                            break;
                        case AggregationType.Max:
                            var measureSavedMax = measure;
                            activeAggregation += s => s.Max(string.Format("Max{0}", measureSavedMax),
                                f => f.Field(ElasticConstants.IndexObjectPath + measureFieldNames[measureSavedMax]));
                            break;
                        case AggregationType.Sum:
                            var measureSavedSum = measure;
                            activeAggregation += s => s.Sum(string.Format("Sum{0}", measureSavedSum),
                                f => f.Field(ElasticConstants.IndexObjectPath + measureFieldNames[measureSavedSum]));
                            break;
                        case AggregationType.Avg:
                            var measureSavedAvg = measure;
                            activeAggregation += s => s.Average(string.Format("Average{0}", measureSavedAvg),
                                f => f.Field(ElasticConstants.IndexObjectPath + measureFieldNames[measureSavedAvg]));
                            break;
                        case AggregationType.Count:
                            var measureSavedCount = measure;
                            activeAggregation += s => s.ValueCount(string.Format("ValueCount{0}", measureSavedCount),
                                f => f.Field(ElasticConstants.IndexObjectPath + "Id"));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("measureFieldName");
                    }
                }
            }



            var queryDimensions = dimensions.ToList();

            var reversedDimensions =  new dynamic[queryDimensions.Count];
            queryDimensions.CopyTo(reversedDimensions);

            // Для формирования измерений проходим по граням куба в обратном порядке.
            // В конечном итоге получится следующее: описание всех агрегаций в требуемом порядке,
            // после чего идет описание измерения
            foreach (var dimension in reversedDimensions.Reverse())
            {
                switch ((DimensionType)dimension.DimensionType)
                {
                    case DimensionType.Range:

                        var rangeDimension = dimension;
                        string rangeAggregationLabel = rangeDimension.Label;
                        string rangeFieldName = ElasticConstants.IndexObjectPath + rangeDimension.FieldName;
                        var rangeAggregation = activeAggregation;

                        var ranges = new List<dynamic>();
                        foreach (var range in CriteriaToRange<double>(dimension.ValueSet))
                        {
                            ranges.Add(new {range.From, range.To}.ToDynamic());
                        }

                        activeAggregation = s => s
                            .Range(rangeAggregationLabel, t => t
                                .Field(rangeFieldName)
                                .Ranges(ranges.Select(RangeSelector).ToArray())
                                .Aggregations(rangeAggregation));
                        break;
                    case DimensionType.DateRange:

                        var dateDimension = dimension;
                        string dateAggregationLabel = dateDimension.Label;
                        string dateFieldName = ElasticConstants.IndexObjectPath + dateDimension.FieldName;
                        var dateAggregation = activeAggregation;
                        string dateFormat = dateDimension.DateTimeFormattingPattern;

                        if (string.IsNullOrEmpty(dateFormat))
                        {
                            dateFormat = "yyyy-MM-dd";
                        }

                        var dateRanges = new List<object>();
                        foreach (var range in CriteriaToRange<DateTime>(dimension.ValueSet))
                        {
                            dateRanges.Add(new { range.From, range.To }.ToDynamic());
                        }

                        activeAggregation = s => s
                            .DateRange(dateAggregationLabel, t => t
                                .Field(dateFieldName)
                                .Format(dateFormat)
                                .Ranges(dateRanges.Select( r => DateRangeSelector(r, dateFormat)).ToArray())
                                .Aggregations(dateAggregation));
                        break;
                    case DimensionType.Term:

                        var termDimension = dimension;
                        var termAggregation = activeAggregation;
                        string termAggregationLabel = termDimension.Label;
                        string termFieldName = ElasticConstants.IndexObjectPath + termDimension.FieldName;

                        activeAggregation =
                            s =>
                                s.Terms(termAggregationLabel,
                                    t => t.Field(termFieldName).Aggregations(termAggregation));
                        break;
                    case DimensionType.DateHistogram:

                        var dateHistogramDimension = dimension;
                        string dateHistogramAggregationLabel = dateHistogramDimension.Label;
                        string dateHistogramFieldName = ElasticConstants.IndexObjectPath + dateHistogramDimension.FieldName;
                        var dateHistogramAggregation = activeAggregation;

                        // Available expressions for interval: year, quarter, month, week, day, hour, minute, second

                        string dateHistogramInterval = dateHistogramDimension.Interval.ToLower();

                        if (string.IsNullOrEmpty(dateHistogramInterval))
                        {
                            dateHistogramInterval = "month";
                        }
                        
                        activeAggregation = s => s
                            .DateHistogram(dateHistogramAggregationLabel, t => t
                                .Field(dateHistogramFieldName)
                                .PreZone(dateHistogramDimension.TimeZone)
                                .Interval(dateHistogramInterval)
                                .Aggregations(dateHistogramAggregation));
                        break;
                }
            }

            // Добавляем фильтр данных, только если он определен
            _filters = filters;
            var hasFilter = _filters is IFilter<FilterContainer>;
            if (hasFilter)
            {
                var filterAggregation = activeAggregation;
                activeAggregation =
                    s => s.Filter("aggrfilter", f => f
                        .Filter(GetFilter)
                        .Aggregations(filterAggregation));
            }

            var rawResult = _elasticConnection.Client.Search<dynamic>(s => s
                    .BuildSearchForType(new[]{_indexName}, _typeName.SelectMany(d => d.TypeNames), _routing, false, false)
                    .Size(0)  // Нас интересуют только агрегации, исключаем результаты поискового запроса
                    .Aggregations(activeAggregation)
                );

            if (!rawResult.IsValid)
            {
                throw new Exception("Failed to calculate aggregation." + rawResult);
            }

            var result = new AggregationResult();
           
            AggregationToEntryResult(
                queryDimensions, 
                measureTypes,
                hasFilter ? rawResult.Aggs.Aggregations["aggrfilter"] as AggregationsHelper : rawResult.Aggs,
                queryDimensions.Count > 0 ? queryDimensions[0] : null, 
                result);

            return result.Buckets;
        }

        /// <summary>
        /// Выполнение агрегирующего запроса
        /// </summary>
        /// <param name="filters">Фильтр для данных</param>
        /// <param name="termFields">Имена полей, по которым будут вычислены Term срезы OLAP куба</param>
        /// <param name="measureType">Тип измерения</param>
        /// <param name="measureFieldName">Имя поля, по которому необходимо произвести вычисление</param>
        /// <returns>Результат выполнения агрегации</returns>
        public IEnumerable<AggregationResult> ExecuteTermAggregation(
            IEnumerable<string> termFields,
            AggregationType measureType,
            string measureFieldName,
            IFilter filters = null)
        {
            var dimensions = termFields.Select(termField => new
            {
                Label = "term_on_" + termField.ToLowerInvariant().Replace(".", ""), 
                FieldName = termField, DimensionType = DimensionType.Term
            }.ToDynamic());

            return ExecuteAggregation(
                dimensions.ToArray(), 
                new []{measureType}, 
                new []{measureFieldName}, 
                filters);
        }

        private void AggregationToEntryResult(
            IList<dynamic> queryDimensions, 
            AggregationType[] measureTypes,
            AggregationsHelper aggregation,
            dynamic dimension,
            AggregationResult entryResultToModify)
        {
            if (dimension == null)
            {
                // срезы куба не заданы, вычисляем результат измерения
                var entryResult = new AggregationResult
                {
                    Name = string.Join("_", measureTypes.Select(mt => mt.ToString())),
                    Values = ExtractCalculations(measureTypes, aggregation),
                    MeasureTypes = measureTypes
                };
                entryResultToModify.Buckets.Add(entryResult);
            }
            else
            {
                switch ((DimensionType) dimension.DimensionType)
                {
                    case DimensionType.Range:
                    case DimensionType.DateRange:
                        for (int i = 0; i < aggregation.Range(dimension.Label).Items.Count; i++)
                        {
                            var item = aggregation.Range(dimension.Label).Items[i];
                            var entryResult = new AggregationResult
                            {
                                Name = RangeToString(item),
                                DocCount = item.DocCount,
                                // по умолчанию MeasureType = Count, если задан другой тип измерения,
                                // то значение перезапишется
                                Values = new double[] { item.DocCount }
                            };

                            var nextDim = NextDimension(queryDimensions, dimension);

                            if (nextDim == null)
                            {
                                entryResult.Values = ExtractCalculations(measureTypes, item);
                                entryResult.MeasureTypes = measureTypes;
                            }
                            else
                            {
                                AggregationToEntryResult(
                                    queryDimensions,
                                    measureTypes,
                                    new AggregationsHelper(item.Aggregations),
                                    nextDim,
                                    entryResult);
                            }

                            entryResultToModify.Buckets.Add(entryResult);
                        }
                        break;
                    case DimensionType.DateHistogram:
                        for (int i = 0; i < aggregation.DateHistogram(dimension.Label).Items.Count; i++)
                        {
                            var item = aggregation.DateHistogram(dimension.Label).Items[i];
                            var entryResult = new AggregationResult
                            {
                                Name = item.KeyAsString,
                                DocCount = item.DocCount,
                                // по умолчанию MeasureType = Count, если задан другой тип измерения,
                                // то значение перезапишется
                                Values = new double[]{item.DocCount}
                            };

                            var nextDim = NextDimension(queryDimensions, dimension);

                            if (nextDim == null)
                            {
                                entryResult.Values = ExtractCalculations(measureTypes, item);
                                entryResult.MeasureTypes = measureTypes;
                            }
                            else
                            {
                                AggregationToEntryResult(
                                    queryDimensions,
                                    measureTypes,
                                    new AggregationsHelper(item.Aggregations),
                                    nextDim,
                                    entryResult);
                            }

                            entryResultToModify.Buckets.Add(entryResult);
                        }
                        break;
                    case DimensionType.Term:
                        foreach (var item in aggregation.Terms(dimension.Label).Items)
                        {
                            var entryResult = new AggregationResult
                            {
                                Name = item.Key,
                                DocCount = item.DocCount,
                                // по умолчанию MeasureType = Count, если задан другой тип измерения,
                                // то значение перезапишется
                                Values = new double[] { item.DocCount }
                            };

                            var nextDim = NextDimension(queryDimensions, dimension);

                            if (nextDim == null)
                            {
                                entryResult.Values = ExtractCalculations(measureTypes, item);
                                entryResult.MeasureTypes = measureTypes;
                            }
                            else
                            {
                                AggregationToEntryResult(
                                    queryDimensions,
                                    measureTypes,
                                    new AggregationsHelper(item.Aggregations),
                                    nextDim,
                                    entryResult);
                            }

                            entryResultToModify.Buckets.Add(entryResult);
                        }
                        break;
                }
            }
        }

        private FilterContainer GetFilter(FilterDescriptor<dynamic> filterDescriptor)
        {
            var filter = (IFilter<FilterContainer>)_filters;
            if (filter == null)
                return null;
            return filter.GetFilterObject();
        }

        /// <summary>
        /// Извлечение результата измерения по его типу
        /// </summary>
        private static double[] ExtractCalculations(IEnumerable<AggregationType> types, AggregationsHelper item)
        {
            var result = new List<double>();

            var measureIndex = 0;

            foreach (var aggregationType in types)
            {
                switch (aggregationType)
                {
                    case AggregationType.Count:
                        var valueCount = item.ValueCount(string.Format("ValueCount{0}", measureIndex)).Value;
                        if (valueCount != null) result.Add(valueCount.Value);
                        break;
                    case AggregationType.Min:
                        var min = item.Min(string.Format("Min{0}", measureIndex)).Value;
                        if (min != null) result.Add(min.Value);
                        break;
                    case AggregationType.Max:
                        var max = item.Max(string.Format("Max{0}", measureIndex)).Value;
                        if (max != null) result.Add(max.Value);
                        break;
                    case AggregationType.Sum:
                        var sum = item.Sum(string.Format("Sum{0}", measureIndex)).Value;
                        if (sum != null) result.Add(sum.Value);
                        break;
                    case AggregationType.Avg:
                        var avg = item.Average(string.Format("Average{0}", measureIndex)).Value;
                        if (avg != null) result.Add(avg.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                measureIndex++;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Переход к следующему измерению
        /// </summary>
        private static dynamic NextDimension(
            IList<dynamic> dimensions,
            dynamic currentDimention)
        {
            var currentIndex = dimensions.IndexOf(currentDimention);

            if (currentIndex + 1 < dimensions.Count)
            {
                return dimensions[currentIndex + 1];
            }

            return null;
        }

        /// <summary>
        /// Позволяет преобразовать диапазон дат в формат, понятный NEST
        /// </summary>
        private static Func<DateExpressionRange, DateExpressionRange> DateRangeSelector(
            dynamic rng,
            string dateStringFormat)
        {
            if (rng.From == null)
            {
                return r => rng.To != null ? r.To(((DateTime) rng.To).ToString(dateStringFormat)) : null;
            }

            if (rng.To == null)
            {
                return r => r.From(((DateTime) rng.From).ToString(dateStringFormat));
            }

            return r => r
                .From(((DateTime) rng.From).ToString(dateStringFormat))
                .To(((DateTime) rng.To).ToString(dateStringFormat));
        }

        /// <summary>
        /// Позволяет преобразовать числовой диапазон в формат, понятный NEST
        /// </summary>
        private Func<Range<double>, Range<double>> RangeSelector(dynamic rng)
        {
            if (rng.From == null)
            {
                return r => rng.To != null ? r.To((double) rng.To) : null;
            }

            if (rng.To == null)
            {
                return r => r.From((double) rng.From);
            }

            return r => r.From((double) rng.From).To((double) rng.To);
        }

        ///  <summary>
        ///  Конструктор для набора диапазонов
        ///  </summary>
        /// <param name="criteria">
        ///  Критерий представляет собой граничную точку диапазона.
        ///  Пример значения:
        ///  
        /// new {
        ///         Property = "Age",
        ///         CriteriaType = CriteriaType.ValueSet,
        ///         Value = 10\n20\n30
        ///     }
        ///  
        ///  В данном случае будет создано три диапазона:
        ///      от минус бесконечности до 10
        ///      от 10 включительно до 20
        ///      от 20 включительно до бесконечности
        ///  </param>
        private IEnumerable<dynamic> CriteriaToRange<T>(dynamic criteria)
        {
            var innerRanges = new List<dynamic>();

            var converter = TypeDescriptor.GetConverter(typeof(T));
            
            var points = new List<T>();
            foreach (string strPoint in criteria.Value.ToString().Split('\n'))
            {
                var convertedValue = converter.ConvertFromString(strPoint.Replace(".", ","));
                if (convertedValue != null)
                {
                    points.Add((T)convertedValue);
                }
            }

            innerRanges.Add(new {To = points[0]}.ToDynamic());

            for (int i = 1; i < points.Count; i++)
            {
                innerRanges.Add(new
                {
                    From = points[i - 1],
                    To = points[i]
                }.ToDynamic());
            }

            innerRanges.Add(new {From = points[points.Count - 1]}.ToDynamic());

            return innerRanges;
        }

        /// <summary>
        /// Форматирует диапазон в строку
        /// </summary>
        private string RangeToString(dynamic range)
        {
            if (range.To == null)
            {
                return string.Format(">={0}", range.FromAsString ?? range.From);
            }

            if (range.From == null)
            {
                return string.Format("<{0}", range.ToAsString ?? range.To);
            }

            return string.Format("{0}-{1}", range.FromAsString ?? range.From, range.ToAsString ?? range.To);
        }
    }
}