using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json
{
	public interface IJsonTokenProvider
	{
		IEnumerable<ParsedToken> GetJsonToken(ParsedToken parsedToken);

	}
}
