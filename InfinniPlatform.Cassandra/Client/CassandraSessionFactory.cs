using System;

using Cassandra;

namespace InfinniPlatform.Cassandra.Client
{
    /// <summary>
    /// Represents session factory for the Apache Cassandra.
    /// </summary>
    sealed class CassandraSessionFactory : ISessionFactory, IDisposable
    {
        public CassandraSessionFactory(CassandraSettings settings)
        {
            _settings = settings;
        }


        private readonly CassandraSettings _settings;


        /// <summary>
        /// Creates a new session.
        /// </summary>
        public ISession OpenSession()
        {
            var session = GetCluster().ConnectAndCreateDefaultKeyspaceIfNotExists();

            return new CassandraSession(session);
        }


        private Cluster _cluster;

        private Cluster GetCluster()
        {
            if (_cluster == null)
            {
                var builder = Cluster.Builder();

                if (_settings.Port > 0)
                {
                    builder.WithPort(_settings.Port);
                }

                if (_settings.Nodes != null && _settings.Nodes.Length > 0)
                {
                    builder.AddContactPoints(_settings.Nodes);
                }
                else
                {
                    builder.AddContactPoint("localhost");
                }

                if (string.IsNullOrWhiteSpace(_settings.Keyspace) == false)
                {
                    builder.WithDefaultKeyspace(_settings.Keyspace);
                }

                _cluster = builder.Build();
            }

            return _cluster;
        }

        private void ShutdownCluster()
        {
            var cluster = _cluster;

            cluster?.Dispose();
        }


        public void Dispose()
        {
            ShutdownCluster();
        }
    }
}