using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Registers;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.SystemConfig.Registers
{
    internal static class AggregationUtils
    {
        /// <summary>
        /// Метод формирует набор измерений, по которым можно проводить агрегацию, на основе метаданных регистра.
        /// </summary>
        public static IEnumerable<dynamic> BuildDimensionsFromRegisterMetadata(dynamic registerObject)
        {
            var dimensions = new List<dynamic>();

            foreach (var property in registerObject.Properties)
            {
                if (property.Type == RegisterPropertyType.Dimension)
                {
                    dimensions.Add(new DynamicWrapper
                                   {
                                       ["Label"] = property.Property + "_term",
                                       ["FieldName"] = property.Property,
                                       ["DimensionType"] = DimensionType.Term
                                   });
                }
            }

            dimensions.Add(new DynamicWrapper
                           {
                               ["Label"] = RegisterConstants.EntryTypeProperty + "_term",
                               ["FieldName"] = RegisterConstants.EntryTypeProperty,
                               ["DimensionType"] = DimensionType.Term
                           });

            return dimensions;
        }

        /// <summary>
        /// Метод формирует набор измерений, по которым можно проводить агрегацию, на основе переданных имен свойств.
        /// </summary>
        public static IEnumerable<dynamic> BuildDimensionsFromProperties(IEnumerable<string> dimensionsProperties)
        {
            var dimensions = new List<dynamic>();

            foreach (dynamic property in dimensionsProperties)
            {
                dimensions.Add(new DynamicWrapper
                               {
                                   ["Label"] = property + "_term",
                                   ["FieldName"] = property,
                                   ["DimensionType"] = DimensionType.Term
                               });
            }

            dimensions.Add(new DynamicWrapper
                           {
                               ["Label"] = RegisterConstants.EntryTypeProperty + "_term",
                               ["FieldName"] = RegisterConstants.EntryTypeProperty,
                               ["DimensionType"] = DimensionType.Term
                           });

            return dimensions;
        }

        /// <summary>
        /// Извлекает имя свойства, хранящего вычисляемое значение (ресурс), на основе метаданных регистра.
        /// </summary>
        public static IEnumerable<string> BuildValuePropertyFromRegisterMetadata(dynamic registerObject)
        {
            var result = new List<string>();

            foreach (var property in registerObject.Properties)
            {
                if (property.Type == RegisterPropertyType.Value)
                {
                    result.Add(property.Property);
                }
            }

            return result;
        }

        public static IEnumerable<AggregationType> BuildAggregationType(AggregationType aggregationType, int countOfValues)
        {
            var result = new AggregationType[countOfValues];

            for (var i = 0; i < countOfValues; i++)
            {
                result[i] = aggregationType;
            }

            return result;
        }

        /// <summary>
        /// Преобразовать комплексный объект, полученный в результате агрегации,
        /// в табличное представление, где имена столбцов будут соответствовать
        /// именам измерений.
        /// </summary>
        public static IEnumerable<dynamic> ProcessBuckets(
            string[] dimensionNames,
            string[] valueProperties,
            IEnumerable<dynamic> buckets)
        {
            var valuesToReturn = new List<dynamic>();

            if (dimensionNames.First() == RegisterConstants.EntryTypeProperty)
            {
                var calculatedValue = new double[valueProperties.Length];
                dynamic denormalizationResult = new DynamicWrapper();

                for (var valuePropertyIndex = 0; valuePropertyIndex < valueProperties.Length; valuePropertyIndex++)
                {
                    foreach (var valueToProcess in buckets)
                    {
                        if (string.Compare(
                            valueToProcess.Name.ToString(),
                            RegisterEntryType.Consumption,
                            StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            calculatedValue[valuePropertyIndex] -= valueToProcess.Values[valuePropertyIndex];
                        }
                        else
                        {
                            // по умолчанию суммируем значения (в том числе и если RegisterEntryType == Income)
                            calculatedValue[valuePropertyIndex] += valueToProcess.Values[valuePropertyIndex];
                        }

                        if (valueToProcess.DenormalizationResult != null)
                        {
                            denormalizationResult = valueToProcess.DenormalizationResult;
                        }
                    }
                }

                for (var valuePropertyIndex = 0; valuePropertyIndex < valueProperties.Length; valuePropertyIndex++)
                {
                    denormalizationResult[valueProperties[valuePropertyIndex]] = calculatedValue[valuePropertyIndex];
                }

                return new[] { denormalizationResult };
            }

            foreach (var valueToProcess in buckets)
            {
                var denormalizationResult = valueToProcess.DenormalizationResult ?? new DynamicWrapper();
                denormalizationResult[dimensionNames.First()] = valueToProcess.Name;

                foreach (var bucket in valueToProcess.Buckets)
                {
                    bucket.DenormalizationResult = denormalizationResult;
                }

                valuesToReturn.AddRange(ProcessBuckets(dimensionNames.Skip(1).ToArray(), valueProperties.ToArray(),
                    valueToProcess.Buckets));
            }

            return valuesToReturn;
        }

        /// <summary>
        /// Метод позволяет объединить результат выполнения агрегации с данными из регистра итогов.
        /// </summary>
        public static IEnumerable<dynamic> MergeAggregaionResults(
            string[] dimensionNames,
            IEnumerable<string> valueProperties,
            IEnumerable<dynamic> processedBuckets,
            IEnumerable<dynamic> aggregationTotals)
        {
            var processedBucketsAsArray = processedBuckets as dynamic[] ?? processedBuckets.ToArray();

            var result = new List<dynamic>(processedBucketsAsArray);

            foreach (var totalEntry in aggregationTotals)
            {
                var hasTotalEntry = false;

                foreach (var entry in processedBucketsAsArray)
                {
                    if (dimensionNames.All(prop => entry[prop] == totalEntry[prop]))
                    {
                        hasTotalEntry = true;
                        if (valueProperties != null)
                        {
                            foreach (var valueProperty in valueProperties)
                            {
                                entry[valueProperty] = totalEntry[valueProperty] + entry[valueProperty];
                            }
                        }
                    }
                }

                if (!hasTotalEntry)
                {
                    totalEntry.Id = null;
                    totalEntry.TimeStamp = null;
                    totalEntry.DocumentDate = null;

                    result.Add(totalEntry);
                }
            }

            return result;
        }
    }
}