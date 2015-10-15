using System;
using System.Windows.Forms;

using InfinniPlatform.QueryDesigner.Forms;

namespace InfinniPlatform.QueryDesigner
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new QueryDesignerForm());
        }
    }
}