using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.WebApi.HttpResultHandlers
{
    public sealed class HttpResultHandlerBadRequest : IHttpResultHandler
    {
        public HttpResponseMessage WrapResult(object result)
        {
            object validProperty = null;
            object isInternalServerError = null;

            var resultAsDynamic = result as IDynamicMetaObjectProvider;
            
            if (resultAsDynamic != null)
            {
                validProperty = resultAsDynamic.GetProperty("IsValid");
                isInternalServerError = resultAsDynamic.GetProperty("IsInternalServerError");
            }

            if (validProperty == null || Convert.ToBoolean(validProperty))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                       {
                           Content = new ObjectContent(typeof (object), result, new JsonMediaTypeFormatter())
                       };
            }

            if (isInternalServerError != null && Convert.ToBoolean(isInternalServerError))
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                       {
                           Content = new ObjectContent(typeof (object), result, new JsonMediaTypeFormatter())
                       };
            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
                   {
                       Content = new ObjectContent(typeof (object), result, new JsonMediaTypeFormatter())
                   };
        }
    }
}
