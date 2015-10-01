namespace InfinniPlatform.Api.RestApi.CommonApi.RouteTraces
{
    public sealed class RouteTraceNoTrace : IRouteTrace
    {
        public void Trace(
            string configuration,
            string metadata,
            string action,
            string url,
            object body,
            string verbType,
            string content)
        {
            //no log
        }
    }
}