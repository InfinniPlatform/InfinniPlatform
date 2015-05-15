namespace InfinniPlatform.Api.RestQuery
{
	public interface IRestQueryExecutor
	{
		RestQueryResponse QueryGet(string url, object queryObject);
		RestQueryResponse QueryPost(string url, object body);
		RestQueryResponse QueryPostFile(string url, object linkedData, string filePath);
		RestQueryResponse QueryPostUrlEncodedData(string url, object formData);
		RestQueryResponse QueryGetUrlEncodedData(string url, object formData);
	}
}
