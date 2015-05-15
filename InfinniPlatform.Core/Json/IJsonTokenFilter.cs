using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
	public interface IJsonTokenFilter
	{
		void FilterJsonToken(JToken jtoken);
	}
}
