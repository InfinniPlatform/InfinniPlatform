using System.Collections.Generic;

namespace InfinniPlatform.DocumentStorage.Services.QueryFactories
{
    internal static class QuerySyntaxParameter
    {
        public const string Search = "search";
        public const string Filter = "filter";
        public const string Select = "select";
        public const string Order = "order";
        public const string Count = "count";
        public const string Skip = "skip";
        public const string Take = "take";

        public static HashSet<string> ParameterNames = new HashSet<string>
                                                       {
                                                           Search,
                                                           Filter,
                                                           Select,
                                                           Order,
                                                           Count,
                                                           Skip,
                                                           Take
                                                       };
    }
}