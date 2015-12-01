namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
    /// <summary>
    /// Настройки подключения к ElasticSearch.
    /// </summary>
	internal sealed class ElasticSearchSettings
    {
        public const string SectionName = "elasticSearch";


        public ElasticSearchSettings()
        {
            Nodes = new[] { "http://localhost:9200" };
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
    }
}