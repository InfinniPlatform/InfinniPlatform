using System.Text;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Metadata;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.MigrationsAndVerifications.Verifications
{
    public sealed class ConfigurationStructureVerification : IConfigurationVerification
    {
        /// <summary>
        ///     Конфигурация, к которой применяется правило проверки
        /// </summary>
        private string _activeConfiguration;

        private IMetadataConfigurationProvider _metadataConfigurationProvider;

        /// <summary>
        ///     Текстовое описание правила проверки
        /// </summary>
        public string Description
        {
            get { return "Checks that configuration contains all required elements"; }
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

            var configReader = new MetadataReaderConfiguration(null);

            dynamic configMetadata = configReader.GetItem(_activeConfiguration);

            if (configMetadata.Documents == null)
            {
                result = false;
                resultMessage.AppendLine("Configuration doesn't contain 'Documents'");
            }

            if (configMetadata.Reports == null)
            {
                result = false;
                resultMessage.AppendLine("Configuration doesn't contain 'Reports'");
            }

            if (configMetadata.Registers == null)
            {
                result = false;
                resultMessage.AppendLine("Configuration doesn't contain 'Registers'");
            }

            if (configMetadata.Menu == null)
            {
                result = false;
                resultMessage.AppendLine("Configuration doesn't contain 'Menu'");
            }

            if (configMetadata.Relations == null)
            {
                result = false;
                resultMessage.AppendLine("Configuration doesn't contain 'Relations'");
            }

            if (configMetadata.Assemblies == null)
            {
                result = false;
                resultMessage.AppendLine("Configuration doesn't contain 'Assemblies'");
            }

            if (configMetadata.Version == null)
            {
                result = false;
                resultMessage.AppendLine("Configuration doesn't contain 'Version'");
            }

            if (result)
            {
                resultMessage.AppendLine("Check succeeded");
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
        }
    }
}