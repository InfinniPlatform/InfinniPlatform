using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.Tests.RestBehavior.Registers
{
    internal sealed class RegisterApiRequest
    {
        public string RegisterName { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public IEnumerable<FilterCriteria> Filter { get; set; }

        public IEnumerable<string> DimensionsProperties { get; set; }

        public IEnumerable<string> ValueProperties { get; set; }

        public IEnumerable<AggregationType> AggregationTypes { get; set; }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public bool Synchronous { get; set; }
    }
}