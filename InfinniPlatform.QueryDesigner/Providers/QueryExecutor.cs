using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.QueryDesigner.Providers
{
	sealed class QueryExecutor
	{
		private string _host;
		private string _port;


		public void SetConnectionSettings(string host, string port)
		{
			_host = host;
			_port = port;
		}
	}
}
