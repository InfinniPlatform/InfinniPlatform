using System.Windows.Controls;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigDeployDesigner
{
	/// <summary>
	/// Элемент управления для развертывания конфигурации.
	/// </summary>
	sealed partial class ConfigDeployDesignerControl : UserControl
	{
		public ConfigDeployDesignerControl()
		{
			InitializeComponent();
		}


		public object Value
		{
			get { return Designer.Value; }
			set { Designer.Value = value; }
		}
	}
}