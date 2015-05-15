using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.QueryDesigner.Contracts
{
	public interface IQueryBlockProvider
	{
		ConstructOrder GetConstructOrder();

		void ProcessQuery(dynamic query);

		bool DefinitionCompleted();
		string GetErrorMessage();
	}
}
