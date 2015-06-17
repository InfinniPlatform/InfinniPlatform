using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;

namespace InfinniPlatform.MetadataDesigner.Views.ProcessTemplates
{
	interface IProcessTemplate
	{
		void OnInitTemplate();

		IEnumerable<HandlerDescription> ActionHandlers { get; set; }

		IEnumerable<HandlerDescription> ValidationHandlers { get; set; }

		IEnumerable<string> ValidationWarnings { get; set; }

		IEnumerable<string> ValidationErrors { get; set; }

		string ConfigId { get; set; }

		string DocumentId { get; set; }

        string Version { get; set; }

		IEnumerable<string> DocumentStates { get; set; }
		IProcessBuilder ProcessBuilder { get; set; }
		dynamic Process { get; set; }

		EditMode EditMode { get; set; }
	}
}
