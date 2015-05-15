using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.RestApi.DataApi
{
	public sealed class DocumentApiUnsecured : DocumentApi
	{
		public DocumentApiUnsecured() : base(false)
		{
			
		}
	}
}
