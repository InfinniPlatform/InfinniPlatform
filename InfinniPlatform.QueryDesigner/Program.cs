using System;
using System.Windows.Forms;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.QueryDesigner.Forms;

namespace InfinniPlatform.QueryDesigner
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            TestApi.StartServer(p => { });

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new QueryDesignerForm());
        }
    }
}