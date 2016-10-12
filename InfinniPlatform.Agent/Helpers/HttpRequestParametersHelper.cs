namespace InfinniPlatform.Agent.Helpers
{
    public static class HttpRequestParametersHelper
    {
        public static int? ToInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return int.Parse(value);
        }
    }
}