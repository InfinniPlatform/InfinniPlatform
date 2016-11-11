using System;

namespace InfinniPlatform.Server.Tasks.Infinni.Node
{
    public static class HttpServiceHelper
    {
        public static int? ParseInt(dynamic value)
        {
            return string.IsNullOrEmpty(value)
                       ? null
                       : int.Parse(value);
        }

        public static string ParseString(dynamic value)
        {
            return string.IsNullOrEmpty(value) || (value == "null")
                       ? null
                       : (string)value;
        }

        public static bool ParseBool(dynamic value)
        {
            return string.Equals((string)value, bool.TrueString, StringComparison.OrdinalIgnoreCase);
        }
    }
}