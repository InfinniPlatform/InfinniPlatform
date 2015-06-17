using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.ContextComponents
{
	public interface IReferenceResolver
	{
		void ResolveReferences(string version, string configId, string documentId, dynamic documents, IEnumerable<dynamic> ignoreResolve);
	}
}
