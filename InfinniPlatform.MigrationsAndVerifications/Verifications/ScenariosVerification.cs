using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.MigrationsAndVerifications.Verifications
{
    public sealed class ScenariosVerification : IConfigurationVerification
    {
        /// <summary>
        ///     Конфигурация, к которой применяется правило проверки
        /// </summary>
        private string _activeConfiguration;

        private IMetadataConfigurationProvider _metadataConfigurationProvider;
        private string _version;

        /// <summary>
        ///     Текстовое описание правила проверки
        /// </summary>
        public string Description
        {
            get { return "Checks that all Action and Verification units exists in configuration assemblies"; }
        }

        /// <summary>
        ///     Идентификатор конфигурации, к которой применима проверка.
        ///     В том случае, если идентификатор не указан (null or empty string),
        ///     проверка применима ко всем конфигурациям
        /// </summary>
        public string ConfigurationId
        {
            get { return ""; }
        }

        /// <summary>
        ///     Версия конфигурации, к которой применимо правило проверки.
        ///     В том случае, если версия не указана (null or empty string),
        ///     правило применимо к любой версии конфигурации
        /// </summary>
        public string ConfigVersion
        {
            get { return ""; }
        }

        /// <summary>
        ///     Выполнить проверку
        /// </summary>
        /// <param name="message">Информативное сообщение с результатом выполнения действия</param>
        /// <returns>Результат выполнения проверки</returns>
        public bool Check(out string message)
        {
            bool result = true;
            var resultMessage = new StringBuilder();

            // Получаем информацию обо всех сценариях из сборок, подцепленных к конфигурации

            IDataReader assemblyMetadataReader =
                new ManagerFactoryConfiguration(_activeConfiguration).BuildAssemblyMetadataReader();

            var scripts = new List<string>();

            foreach (dynamic appliedAssembly in assemblyMetadataReader.GetItems())
            {
                Assembly sourceAssembly = LoadAppliedAssembly(appliedAssembly.Name.ToString());

                if (sourceAssembly != null)
                {
                    scripts.AddRange(BuildScriptNames(sourceAssembly));
                }
            }

            // Считываем имена всех сценариев, хранящихся в метаднных конфигурации и
            // проверяем, что сценарии имеются в списке scripts, сформированном ранее

            IDataReader documentMetadataReader =
                new ManagerFactoryConfiguration(_activeConfiguration).BuildDocumentMetadataReader();

            foreach (dynamic document in documentMetadataReader.GetItems())
            {
                if (document.Name.ToString() == "Common")
                {
                    // не выполняем проверку для служебного документа Common
                    continue;
                }

                IDataReader scenariosReader =
                    new ManagerFactoryDocument(_activeConfiguration, document.Name.ToString())
                        .BuildScenarioManager().MetadataReader;

                foreach (dynamic scenario in scenariosReader.GetItems())
                {
                    string match = scripts.FirstOrDefault(
                        s => string.Compare(s, scenario.Name.ToString(), StringComparison.OrdinalIgnoreCase) == 0);

                    if (match == null)
                    {
                        result = false;
                        resultMessage.AppendLine();
                        resultMessage.AppendFormat("Scenario {0} was not found in configuration assemblies.",
                                                   scenario.Name.ToString());
                    }
                    else
                    {
                        resultMessage.AppendLine();
                        resultMessage.AppendFormat("Scenario {0} was found in configuration assemblies.",
                                                   scenario.Name.ToString());
                    }
                }
            }

            if (result)
            {
                resultMessage.AppendLine();
                resultMessage.AppendFormat("Check succeeded");
            }

            message = resultMessage.ToString();

            return result;
        }

        /// <summary>
        ///     Устанавливает активную конфигурацию для правила проверки
        /// </summary>
        public void AssignActiveConfiguration(string version, string configurationId, IGlobalContext context)
        {
            _activeConfiguration = configurationId;
            _version = version;
        }

        private static Assembly LoadAppliedAssembly(string assemblyName)
        {
            string pathToAssemblies = AppDomain.CurrentDomain.BaseDirectory;

            string assemblyFileName = Path.Combine(pathToAssemblies, assemblyName);
            if (File.Exists(assemblyFileName + ".dll"))
            {
                return Assembly.LoadFile(assemblyFileName + ".dll");
            }

            if (File.Exists(assemblyFileName + ".exe"))
            {
                return Assembly.LoadFile(assemblyFileName + ".exe");
            }

            return null;
        }

        private static IEnumerable<string> BuildScriptNames(Assembly assembly)
        {
            var result = new List<string>();

            var scriptInfoProvider = new ScriptInfoProvider(assembly);
            IEnumerable<dynamic> infoList = scriptInfoProvider.GetScriptMethodsInfo();

            result.AddRange(infoList.Select(o => (string) o.TypeName.ToString()));

            return result;
        }
    }
}