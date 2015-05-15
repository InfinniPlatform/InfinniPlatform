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
					exportConfig.ExportJsonConfigToDirectory(configurationPath));
					process.EndOperation();

					MessageBox.Show(string.Format("Экспорт конфигурации в каталог \"{0}\" завершен", configurationPath));
				}
			}
			else
			{
				var dialog = new SaveFileDialog
							 {
								 FileName = Value.Name,
								 AddExtension = true,
								 DefaultExt = "zip",
								 Filter = @"Archive files (*.zip)|",
								 InitialDirectory = AppSettings.GetValue("AppliedAssemblies") ?? Directory.GetCurrentDirectory()
							 };

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var process = new StatusProcess();
					process.StartOperation(() => exportConfig.ExportJsonConfigToZip(dialog.FileName));
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
	                () =>
	                    new ExchangeDirector(new ExchangeRemoteHost(GetHostingConfig(), TextEditVersionName.Text),
	                        Value.Name).UpdateConfigurationMetadataFromDirectory(dialog.SelectedPath));
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
				process.StartOperation(() => new ExchangeDirector(new ExchangeRemoteHost(GetHostingConfig(), TextEditVersionName.Text), Value.Name).UpdateConfigurationMetadataFromZip(dialog.FileName));
				process.EndOperation();

				MessageBox.Show(@"Импорт конфигурации завершен");
			}
		}

		private void ButtonUpdateConfigClick(object sender, EventArgs eventArgs)
		{
			var process = new StatusProcess();
			process.StartOperation(() => new ExchangeDirector(new ExchangeRemoteHost(GetHostingConfig(), TextEditVersionName.Text), Value.Name).UpdateConfigurationMetadataFromSelf());
			process.EndOperation();
		}

        private void ButtonUpdateConfigurationAppliedAssemblies_Click(object sender, EventArgs e)
        {
            var configuration = textEditConfigurationNameToUpdateAssemblies.Text;

            if (string.IsNullOrEmpty(configuration))
            {
                MessageBox.Show(@"Укажите наименование конфигурации, для которой необходимо обновить прикладные сборки.");
                return;
            }

            var process = new StatusProcess();

            process.StartOperation(() => new ExchangeDirector(
                new ExchangeRemoteHost(GetHostingConfig(), TextEditVersionName.Text), Value.Name)
                .UpdateConfigurationAppliedAssemblies(configuration));

            process.EndOperation();
        }

		void ButtonRefreshConfigLocalClick(object sender, EventArgs e)
		{
			var process = new StatusProcess();
			process.StartOperation(() => new ExchangeDirector(new ExchangeLocalHost(), Value.Name).UpdateConfigurationMetadataFromSelf());
			process.EndOperation();
		}

		public dynamic Value { get; set; }
        
	}
}