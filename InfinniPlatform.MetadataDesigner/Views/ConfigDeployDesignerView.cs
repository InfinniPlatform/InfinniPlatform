using System;
using System.IO;
using System.Windows.Forms;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.MetadataDesigner.Views.Exchange;
using InfinniPlatform.MetadataDesigner.Views.Status;
using InfinniPlatform.MetadataDesigner.Views.Update;

namespace InfinniPlatform.MetadataDesigner.Views
{
	/// <summary>
	/// Элемент управления для развертывания конфигурации.
	/// </summary>
	public sealed partial class ConfigDeployDesignerView : UserControl
	{
		public ConfigDeployDesignerView()
		{
			InitializeComponent();
		}

		private void ButtonExportConfigClick(object sender, EventArgs e)
		{
			var exportConfig = new ExchangeDirector(new ExchangeLocalHost(), Value.Name);

			if (toggleDirectory.IsOn)
			{
				var dialog = new FolderBrowserDialog();

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var configurationPath = dialog.SelectedPath;
					var process = new StatusProcess();
					process.StartOperation(() =>
					{
					    configurationPath = Path.Combine(configurationPath,
					        string.Format("{0}.Configuration.{1}", Value.Name, Value.Version));
					    exportConfig.ExportJsonConfigToDirectory(configurationPath, Value.Version);
					});
					process.EndOperation();

					MessageBox.Show(string.Format("Экспорт конфигурации в каталог \"{0}\" завершен", configurationPath));
				}
			}
			else
			{
				var dialog = new SaveFileDialog
							 {
                                 FileName = string.Format("{0}.Configuration.{1}.zip", Value.Name, Value.Version),
								 AddExtension = true,
								 DefaultExt = "zip",
								 Filter = @"Archive files (*.zip)|",
								 InitialDirectory = AppSettings.GetValue("AppliedAssemblies") ?? Directory.GetCurrentDirectory()
							 };

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var process = new StatusProcess();
					process.StartOperation(() => exportConfig.ExportJsonConfigToZip(dialog.FileName, Value.Version));
					process.EndOperation();

					MessageBox.Show(@"Экспорт конфигурации завершен");
				}
			}
		}

		private void ImportButtonClick(object sender, EventArgs e)
		{
			if (toggleDirectory.IsOn)
			{
				ImportConfigurationFromDirectory();
			}
			else
			{
				ImportConfigurationFromZip();
			}
		}

	    private void ImportConfigurationFromDirectory()
	    {
	        var dialog = new FolderBrowserDialog();

	        if (dialog.ShowDialog() == DialogResult.OK)
	        {
	            var process = new StatusProcess();
	            process.StartOperation(
	                () => CreateExchangeDirector().UpdateConfigurationMetadataFromDirectory(dialog.SelectedPath));
	            process.EndOperation();

	            MessageBox.Show(@"Импорт конфигурации завершен");
	        }
	    }

	    private HostingConfig GetHostingConfig()
		{
			return new HostingConfig { ServerName = GetServerName(), ServerPort = GetServerPort() };
		}

		private string GetServerName()
		{
			return TextEditServerName.Text;
		}

		private int GetServerPort()
		{
			int port;
			int.TryParse(TextEditServerPort.Text, out port);
			return port;
		}

		private void ImportConfigurationFromZip()
		{
			var dialog = new OpenFileDialog
						 {
							 AddExtension = true,
							 DefaultExt = "zip",
							 Filter = @"Archive files (*.zip)|",
							 InitialDirectory = AppSettings.GetValue("AppliedAssemblies") ?? Directory.GetCurrentDirectory()
						 };

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				var process = new StatusProcess();
				process.StartOperation(() => CreateExchangeDirector().UpdateConfigurationMetadataFromZip(dialog.FileName));
				process.EndOperation();

				MessageBox.Show(@"Импорт конфигурации завершен");
			}
		}

	    private ExchangeDirector CreateExchangeDirector()
	    {
	        return new ExchangeDirector(new ExchangeRemoteHost(GetHostingConfig(),Value.Version), Value.Name);
	    }

	    private void ButtonUpdateConfigurationAppliedAssemblies_Click(object sender, EventArgs e)
        {
            var process = new StatusProcess();

            process.StartOperation(() => CreateExchangeDirector()
                .UpdateConfigurationAppliedAssemblies());

            process.EndOperation();
        }


		public dynamic Value { get; set; }
        
	}
}