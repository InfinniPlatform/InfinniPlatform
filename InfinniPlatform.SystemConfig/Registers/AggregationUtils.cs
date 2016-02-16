using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Registers;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.SystemConfig.Registers
{
    internal static class AggregationUtils
    {
        /// <summary>
        /// Метод формирует набор измерений, по которым можно проводить агрегацию, на основе метаданных регистра.
        /// </summary>
        public static IEnumerable<string> BuildDimensionsFromRegisterMetadata(dynamic registerObject)
        {
            var dimensions = new List<string>();

            foreach (var property in registerObject.Properties)
            {
                if (property.Type == RegisterPropertyType.Dimension)
                {
                    dimensions.Add(property.Property);
                }
            }

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