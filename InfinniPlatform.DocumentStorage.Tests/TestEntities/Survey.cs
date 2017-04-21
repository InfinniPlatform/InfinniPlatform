using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.TestEntities
{
    internal class Survey
    {
        public object _id { get; set; }

        public IEnumerable<SurveyResult> results { get; set; }
    }
}