using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.Hosting.WebApi.Implementation.HttpResultHandlers
{
	public sealed class HttpResultHandlerBadRequest : IHttpResultHandler
	{
		public HttpResponseMessage WrapResult(object result)
		{
			dynamic target = result;
			var isValidInstance = DynamicInstanceExtensions.ToDynamic(target).GetProperty("IsValid");
			var isValid = isValidInstance == null || (bool)isValidInstance;
			if (isValid)
			{
				return new HttpResponseMessage(HttpStatusCode.OK)
					       {
						       Content = new ObjectContent(typeof (object),result != null ? result.ToSerializable() : null, new JsonMediaTypeFormatter())
					       };
			}

			return new HttpResponseMessage(HttpStatusCode.BadRequest)
				       {
						   Content = new ObjectContent(typeof(object), result != null ? result.ToSerializable() : null, new JsonMediaTypeFormatter())
				       };
		}
	}
}
