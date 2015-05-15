using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.RestfulApi.Utils
{
	internal sealed class DocumentLink
	{
		public string ConfigId { get; set; }

		public string DocumentId { get; set; }

		public string InstanceId { get; set; }

		public Action<dynamic> SetValue { get; set; }
	}
}
