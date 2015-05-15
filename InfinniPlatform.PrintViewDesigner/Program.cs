using System;
using System.Windows;

using InfinniPlatform.PrintViewDesigner.Views;

namespace InfinniPlatform.PrintViewDesigner
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			new Application { MainWindow = new MainWindow { Visibility = Visibility.Visible } }.Run();
		}
	}
}