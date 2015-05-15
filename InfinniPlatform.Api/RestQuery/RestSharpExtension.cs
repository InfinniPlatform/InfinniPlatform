using RestSharp;

namespace InfinniPlatform.Api.RestQuery
{
	public static class RestSharpExtension
	{
		public static RestQueryResponse ToQueryResponse(this IRestResponse restResponse)
		{
			return new RestQueryResponse()
				       {
					       Content = restResponse.Content,
					       HttpStatusCode = restResponse.StatusCode,
				       };
		}
	}
}
