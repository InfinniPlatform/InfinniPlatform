using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.Hosting.WebApi.Implementation.HttpResultHandlers
{
	public sealed class HttpResultHandlerStandard : IHttpResultHandler
	{
		public HttpResponseMessage WrapResult(object result)
		{
			return new HttpResponseMessage(HttpStatusCode.OK)
				       {
					       Content = new ObjectContent(typeof(object), result != null ? result.ToSerializable() : null, new JsonMediaTypeFormatter())
				       };
		}
	}
}
