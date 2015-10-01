using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using InfinniPlatform.ReportDesigner.Views.Designer;

namespace InfinniPlatform.ReportDesigner
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ReportDesignerForm());
        }
    }
}