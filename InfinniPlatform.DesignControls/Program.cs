using System;
using System.Windows.Forms;

namespace InfinniPlatform.DesignControls
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				var hostProcesses = System.Diagnostics.Process.GetProcessesByName("InfinniPlatform.RestfulApi");

				if (hostProcesses.Length == 0)
				{
					Api.TestEnvironment.TestApi.StartServer(p => { });
				}
			}
#endif

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var form = new Form();
			var designerControl = new DesignerControl();
			designerControl.Dock = DockStyle.Fill;
			form.Controls.Add(designerControl);
			form.Width = 1024;
			form.Height = 768;
			Application.Run(form);
		}
	}
}
