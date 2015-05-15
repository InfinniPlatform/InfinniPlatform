using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace InfinniPlatform.WebApi.HttpResultHandlers
{
	public sealed class HttpResultHandlerStandard : IHttpResultHandler
	{
		public HttpResponseMessage WrapResult(object result)
		{
			return new HttpResponseMessage(HttpStatusCode.OK)
				       {
					       Content = new ObjectContent(typeof(object), result, new JsonMediaTypeFormatter())
				       };
		}
	}
}
