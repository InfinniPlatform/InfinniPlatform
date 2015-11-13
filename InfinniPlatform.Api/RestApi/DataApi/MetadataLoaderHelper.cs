namespace InfinniPlatform.Api.RestApi.DataApi
{
    public static class MetadataLoaderHelper
    {
        public static object TryGetValueContent(dynamic dictionary, string docId)
        {
            dynamic item;
            return dictionary.TryGetValue(docId, out item)
                       ? item.Content
                       : null;
        }

        public static object TryGetValue(dynamic dictionary, string docId)
        {
            dynamic item;
            return dictionary.TryGetValue(docId, out item)
                       ? item
                       : null;
        }
    }
}