using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.Environment.Hosting;
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
