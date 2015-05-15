using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace InfinniPlatform.WebApi.HttpResultHandlers
{
	sealed class HttpResultHandlerByteContent : IHttpResultHandler
	{
		public HttpResponseMessage WrapResult(object result)
		{
			dynamic res = result;
			if (res.IsValid != null && !res.IsValid)
			{
				var response = new HttpResponseMessage();
				response.StatusCode = HttpStatusCode.BadRequest;
				response.Content = new StringContent(result.ToString(),Encoding.UTF8);
				response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
				response.Content.Headers.ContentEncoding.Add("UTF-8");
				return response;
			}

			dynamic resultBytes = null;
			if (res.Result != null)
			{
				resultBytes = res.Result;
			}
			else
			{
				resultBytes = result;
			}
            
			return new HttpResponseMessage(HttpStatusCode.OK)
			{
                Content = new ByteArrayContent(resultBytes.Data as byte[])
				{
					Headers =
					{
						ContentLength = resultBytes.Info.Size,
						ContentType = new MediaTypeHeaderValue(resultBytes.Info.Type)
					}
				}
			};
		}
	}
}
