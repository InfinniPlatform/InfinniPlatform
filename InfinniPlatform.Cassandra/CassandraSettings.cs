namespace InfinniPlatform.Cassandra
{
    /// <summary>
    /// Настройки подключения к Apache Cassandra.
    /// </summary>
    public sealed class CassandraSettings
    {
        public static readonly CassandraSettings Default = new CassandraSettings();


        /// <summary>
        /// Список рабочих узлов по умолчанию.
        /// </summary>
        private static readonly string[] DefaultNodes = { "localhost" };

        /// <summary>
        /// Порт для подключения по умолчанию.
        /// </summary>
        private const int DeafultPort = 9042;

        /// <summary>
        /// Пространство ключей по умолчанию.
        /// </summary>
        private const string DeafultKeyspace = "system";


        public CassandraSettings()
        {
            Nodes = DefaultNodes;
            Port = DeafultPort;
            Keyspace = DeafultKeyspace;
        }


        /// <summary>
        /// Список рабочих узлов.
        /// </summary>
        public string[] Nodes { get; set; }

        /// <summary>
        /// Порт для подключения.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Пространство ключей.
        /// </summary>
        public string Keyspace { get; set; }
    }
}