using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Schema
{
	public interface ISchemaProvider
	{
		dynamic GetSchema(string configId, string documentId);
	}
}
