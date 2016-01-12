using InfinniPlatform.Core.Hosting;
using InfinniPlatform.WebApi.HttpResultHandlers;

namespace InfinniPlatform.WebApi.Factories
{
    public sealed class HttpResultHandlerFactory : IHttpResultHandlerFactory
    {
        public IHttpResultHandler GetResultHandler(HttpResultHandlerType httpResultHandlerType)
        {
            if (httpResultHandlerType == HttpResultHandlerType.BadRequest)
            {
                return new HttpResultHandlerBadRequest();
            }
            if (httpResultHandlerType == HttpResultHandlerType.Html)
            {
                return new HttpResultHandlerHtml();
            }
            if (httpResultHandlerType == HttpResultHandlerType.ByteContent)
            {
                return new HttpResultHandlerByteContent();
            }
            if (httpResultHandlerType == HttpResultHandlerType.SignIn)
            {
                return new HttpResultHandlerSignIn();
            }
            return new HttpResultHandlerStandard();
        }
    }
}