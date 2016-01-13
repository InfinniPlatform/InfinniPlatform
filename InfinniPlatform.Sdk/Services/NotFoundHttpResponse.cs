namespace InfinniPlatform.Sdk.Services
{
    public sealed class NotFoundHttpResponse : HttpResponse
    {
        public static readonly NotFoundHttpResponse Instance = new NotFoundHttpResponse();


        public NotFoundHttpResponse()
        {
            StatusCode = 404;
        }
    }
}