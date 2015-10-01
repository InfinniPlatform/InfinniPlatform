using System;
using System.Diagnostics;
using System.Windows.Forms;
using InfinniPlatform.Api.TestEnvironment;

namespace InfinniPlatform.DesignControls
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
#if DEBUG
            if (Debugger.IsAttached)
            {
                var hostProcesses = Process.GetProcessesByName("InfinniPlatform.RestfulApi");

                if (hostProcesses.Length == 0)
                {
                    TestApi.StartServer(p => { });
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