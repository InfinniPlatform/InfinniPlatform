using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.Index
{
    /// <summary>
    ///     Результат выполнения агрегации по одному измерению
    /// </summary>
    public class AggregationResult
    {
        private readonly List<AggregationResult> _backets;

        public AggregationResult()
        {
            _backets = new List<AggregationResult>();
        }

        /// <summary>
        ///     Gets an aggregation name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets measure type of aggregation
        /// </summary>
        public AggregationType[] MeasureTypes { get; set; }

        /// <summary>
        ///     Measured aggregation values
        /// </summary>
        public double[] Values { get; set; }

        /// <summary>
        ///     Count of documents in the aggregation
        /// </summary>
        public long? DocCount { get; set; }

        /// <summary>
        ///     Gets the inner aggregation result
        /// </summary>
        public List<AggregationResult> Buckets
        {
            get { return _backets; }
        }

        //TODO Избавиться в процессе рефакторинга регистров.
        /// <remarks>
        /// Раньше появлялось только в динамическом контексте. Поведение до конца не изучено.
        /// </remarks>
        public DynamicWrapper DenormalizationResult { get; set; }
    }
}