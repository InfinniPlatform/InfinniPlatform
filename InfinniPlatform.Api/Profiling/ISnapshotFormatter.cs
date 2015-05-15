using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Profiling
{
	public interface ISnapshotFormatter
	{
		void FormatSnapshot(Snapshot snapshot);
	}
}
