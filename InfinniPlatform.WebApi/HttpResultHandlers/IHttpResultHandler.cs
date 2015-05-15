using System.Net.Http;

namespace InfinniPlatform.WebApi.HttpResultHandlers
{
	public interface IHttpResultHandler
	{
		HttpResponseMessage WrapResult(object result);
	}
}
