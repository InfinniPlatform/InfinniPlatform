using System;
using System.IO;
using System.Windows.Forms;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.MetadataDesigner.Views.Exchange;
using InfinniPlatform.MetadataDesigner.Views.Status;
using InfinniPlatform.MetadataDesigner.Views.Update;
using InfinniPlatform.Sdk.Api;

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

        private string GetSolutionName()
        {
            return TextEditSolutionName.Text;
        }

	    private string GetNewSolutionVersion()
	    {
	        return TextEditSolutionVersionNew.Text;
	    }

        private string GetSolutionVersion()
        {
            return TextEditSolutionVersion.Text;
        }

        private void ButtonImportSolutionClick(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var exchangeDirector = new ExchangeDirectorSolution(new ExchangeLocalHost(), GetSolutionName());

                var process = new StatusProcess();
                process.StartOperation(
                    () =>
                        {
                            exchangeDirector.UpdateSolutionMetadataFromDirectory(dialog.SelectedPath);
                        });
                process.EndOperation();

                MessageBox.Show(@"Импорт конфигурации завершен");
            }
        }

        private void ButtonExportSolutionClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GetSolutionName()) && !string.IsNullOrEmpty(GetSolutionVersion()) && !string.IsNullOrEmpty(GetNewSolutionVersion()))
            {
                var exportSolution = new ExchangeDirectorSolution(new ExchangeLocalHost(), GetSolutionName());

                var dialog = new FolderBrowserDialog();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var solutionPath = dialog.SelectedPath;
                    var process = new StatusProcess();
                    process.StartOperation(() =>
                        {
                            solutionPath = Path.Combine(solutionPath, GetSolutionName());
                            exportSolution.ExportJsonSolutionToDirectory(solutionPath, GetSolutionVersion(), GetNewSolutionVersion());

                            var manager = ManagerFactorySolution.BuildSolutionReader(GetSolutionVersion());

                            dynamic solution = manager.GetItem(GetSolutionName());

                            foreach (var config in solution.ReferencedConfigurations)
                            {
                                var exportConfig = new ExchangeDirector(new ExchangeLocalHost(), config.Name);
                                var configurationPath = Path.Combine(solutionPath,
                                    string.Format("{0}_{1}", config.Name, config.Version));
                                exportConfig.ExportJsonConfigToDirectory(configurationPath, Value.Version,GetNewSolutionVersion());
                            }

                        });
                    process.EndOperation();

                    MessageBox.Show(string.Format("Export solution into folder \"{0}\" completed.", solutionPath));
                }
            }
            else
            {
                MessageBox.Show(string.Format("Solution name and version should not be empty."));
            }
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
					        string.Format("{0}.Configuration_{1}", Value.Name, Value.Version));
					    exportConfig.ExportJsonConfigToDirectory(configurationPath, Value.Version,GetNewSolutionVersion());
					});
					process.EndOperation();

					MessageBox.Show(string.Format("Экспорт конфигурации в каталог \"{0}\" завершен", configurationPath));
				}
			}
			else
			{
				var dialog = new SaveFileDialog
							 {
                                 FileName = string.Format("{0}_{1}.zip", Value.Name, Value.Version),
								 AddExtension = true,
								 DefaultExt = "zip",
								 Filter = @"Archive files (*.zip)|",
								 InitialDirectory = AppSettings.GetValue("AppliedAssemblies") ?? Directory.GetCurrentDirectory()
							 };

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var process = new StatusProcess();
					process.StartOperation(() => exportConfig.ExportJsonConfigToZip(dialog.FileName, Value.Version,GetNewSolutionVersion()));
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