using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Profiling.Implementation
{
	public sealed class ActionUnitProfiler : IOperationProfiler
	{
		private WatchProfiler _profiler;

		public ActionUnitProfiler(ILog log, string methodName, string arguments)
		{
			_profiler = new WatchProfiler(new ActionUnitFormatterLog(log, methodName, arguments));
		}

		public void Reset()
		{
			_profiler.Reset();
		}

		public void TakeSnapshot()
		{
			_profiler.TakeSnapshot();
		}
	}
}
