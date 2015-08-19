using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.MetadataDesigner.Views.Exchange;
using InfinniPlatform.MetadataDesigner.Views.Update;
using InfinniPlatform.Sdk.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Utils
{
    public class ConfigManager
    {
        public void Upload(string solutionDir, bool uploadMetadata)
        {
            Console.WriteLine("ServiceHost should be executed for this operation.");
            Console.WriteLine("All metadata will be DESTROYED!!! Are you sure?");
            if (uploadMetadata && Console.ReadKey().KeyChar != 'y')
            {
                Console.WriteLine("Cancel upload");
                return;
            }

            ProcessSolution(solutionDir, solution =>
            {
                Console.WriteLine("Uploading solution '{0}' started", solution.Name);

                ExchangeDirectorSolution exchangeDirector = CreateExchangeDirector(solution.Name.ToString(), solution.Version.ToString());

                dynamic solutionData = null;
                if (uploadMetadata)
                {
                    exchangeDirector.UpdateSolutionMetadataFromDirectory(solutionDir);
                }

                exchangeDirector.UpdateConfigurationAppliedAssemblies(solution);

                Console.WriteLine("Uploading solution '{0}' done", solution.Name);
            });
        }

        public void Download(string solutionDir, string solution, string version, string newVersion)
        {
            Console.WriteLine("Downloading solution '{0}' started", solution);

            var exchangeDirector = CreateExchangeDirector(solution,version);
            exchangeDirector.ExportJsonSolutionToDirectory(solutionDir, version, newVersion);

            Console.WriteLine("Downloading solution '{0}' done", solution);

        }

        private static void ProcessSolution(string solutionDir, Action<dynamic> action)
        {

            var solutionFile = Directory.GetFiles(solutionDir)
                     .FirstOrDefault(file => file.ToLowerInvariant().Contains("solution.json"));

            if (solutionFile != null)
            {
                var jsonSolution = JObject.Parse(File.ReadAllText(solutionFile));
                action(jsonSolution);
            }
        }

        private static ExchangeDirectorSolution CreateExchangeDirector(string configName, string version)
        {
            var remoteHost = new ExchangeRemoteHost(new HostingConfig(), version);
            return new ExchangeDirectorSolution(remoteHost, configName);
        }
    }
}