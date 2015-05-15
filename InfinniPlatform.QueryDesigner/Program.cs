using System;
using System.Windows.Forms;

using InfinniPlatform.QueryDesigner.Forms;

namespace InfinniPlatform.QueryDesigner
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Api.TestEnvironment.TestApi.StartServer(p => { });

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new QueryDesignerForm());
		}
	}
}