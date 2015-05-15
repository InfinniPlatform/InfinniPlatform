using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Profiling.Implementation
{
	sealed class ActionUnitFormatterLog : ISnapshotFormatter
	{
		private readonly ILog _log;
		private readonly string _methodName;
		private readonly string _arguments;

		public ActionUnitFormatterLog(ILog log, string methodName, string arguments)
		{
			_log = log;
			_methodName = methodName;
			_arguments = arguments;
		}

		public void FormatSnapshot(Snapshot snapshot)
		{
			_log.Info("Run method {0} with arguments {1}. ELAPSED {2} ms", _methodName,_arguments,snapshot.ElapsedMilliseconds);
		}
	}
}
