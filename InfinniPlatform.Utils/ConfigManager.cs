using System;
using System.Configuration;
using System.IO;
using System.Linq;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.MetadataDesigner.Views.Exchange;
using InfinniPlatform.MetadataDesigner.Views.Update;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.Utils
{
    public class ConfigManager
    {
        public void Upload(string config, bool uploadMetadata)
        {
            Console.WriteLine("ServiceHost should be executed for this operation.");
            Console.WriteLine("All metadata will be DESTROYED!!! Are you sure?");
            if (uploadMetadata && Console.ReadKey().KeyChar != 'y')
            {
                Console.WriteLine("Cancel upload");
                return;
            }

            ProcessConfigurations(config, configuration =>
            {
                Console.WriteLine("Uploading configuration '{0}' started", configuration.Name);

                var exchangeDirector = CreateExchangeDirector(AdjustConfigName(configuration.Name),
                    configuration.Version);

                if (uploadMetadata)
                    exchangeDirector.UpdateConfigurationMetadataFromDirectory(configuration.PathString);

                exchangeDirector.UpdateConfigurationAppliedAssemblies();

                Console.WriteLine("Uploading configuration '{0}' done", configuration.Name);
            });
        }

        private static string AdjustConfigName(string name)
        {
            return name.Split('.').Last();
        }

        public void Download(string config)
        {
            ProcessConfigurations(config, configuration =>
            {
                Console.WriteLine("Downloading configuration '{0}' started", configuration.Name);

                var exchangeDirector = CreateExchangeDirector(configuration.Name, configuration.Version);
                exchangeDirector.ExportJsonConfigToDirectory(configuration.PathString, configuration.Version);

                Console.WriteLine("Downloading configuration '{0}' done", configuration.Name);
            });
        }

        private static void ProcessConfigurations(string config, Action<Configuration> action)
        {
            var neededConfigs = config == null ? new string[0] : config.ToLower().Split(',');
            var configurations = Directory
                .GetDirectories(ConfigurationManager.AppSettings["ConfigurationsDir"])
                .Where(dir => dir.Contains(".Configuration"))
                .Select(dir => new Configuration(dir))
                .Where(c => !neededConfigs.Any() || neededConfigs.Contains(c.Name.ToLower()));

            foreach (var configuration in configurations)
            {
                action(configuration);
            }
        }

        private static ExchangeDirector CreateExchangeDirector(string configName, string version)
        {
            var remoteHost = new ExchangeRemoteHost(new HostingConfig(), version);
            return new ExchangeDirector(remoteHost, configName);
        }
    }
}