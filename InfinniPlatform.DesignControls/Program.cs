using System;
using System.Windows.Forms;

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