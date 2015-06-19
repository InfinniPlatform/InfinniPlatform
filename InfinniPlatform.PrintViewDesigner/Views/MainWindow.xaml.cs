using System.Windows;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.PrintViewDesigner.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Designer.PrintView = new DynamicWrapper();
        }
    }
}