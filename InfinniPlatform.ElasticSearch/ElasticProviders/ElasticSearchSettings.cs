namespace InfinniPlatform.ElasticSearch.ElasticProviders
{
    /// <summary>
    /// Настройки подключения к ElasticSearch.
    /// </summary>
    public sealed class ElasticSearchSettings
    {
        public const string SectionName = "elasticSearch";

        public static readonly ElasticSearchSettings Default = new ElasticSearchSettings();

        public ElasticSearchSettings()
        {
            Nodes = new[] { "http://localhost:9200" };
            EnableTrace = false;
            EnableResolve = true;
        }

        /// <summary>
        /// Список базовых адресов узлов ElasticSearch.
        /// </summary>
        public string[] Nodes { get; set; }

        /// <summary>
        /// Логин для доступа к ElasticSearch через Shield.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Пароль для доступа к ElasticSearch через Shield.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Разрешить трассировку запросов к ElasticSearch.
        /// </summary>
        public bool EnableTrace { get; set; }

        /// <summary>
        /// Разрешить resolve вложенных документов.
        /// </summary>
        public bool EnableResolve { get; set; }
    }
}