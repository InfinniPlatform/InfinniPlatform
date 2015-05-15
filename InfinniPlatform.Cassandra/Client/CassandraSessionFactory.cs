using System;

using Cassandra;
using InfinniPlatform.Api.Settings;

namespace InfinniPlatform.Cassandra.Client
{
	/// <summary>
	/// Represents session factory for the Apache Cassandra.
	/// </summary>
	sealed class CassandraSessionFactory : ISessionFactory, IDisposable
	{
		public CassandraSessionFactory()
		{
			_contactPort = AppSettings.GetValue<int>(ContactPortConfig);
			_contactPoints = AppSettings.GetValues<string>(ContactPointsConfig);
			_defaultKeyspace = AppSettings.GetValue(DefaultKeyspaceConfig, "system");
		}


		private readonly int _contactPort;
		private readonly string[] _contactPoints;
		private readonly string _defaultKeyspace;

		public const string ContactPortConfig = "CassandraContactPort";
		public const string ContactPointsConfig = "CassandraContactPoints";
		public const string DefaultKeyspaceConfig = "CassandraDefaultKeyspace";


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

				if (_contactPort > 0)
				{
					builder.WithPort(_contactPort);
				}

				if (_contactPoints != null && _contactPoints.Length > 0)
				{
					builder.AddContactPoints(_contactPoints);
				}
				else
				{
					builder.AddContactPoint("localhost");
				}

				if (string.IsNullOrWhiteSpace(_defaultKeyspace) == false)
				{
					builder.WithDefaultKeyspace(_defaultKeyspace);
				}

				_cluster = builder.Build();
			}

			return _cluster;
		}

		private void ShutdownCluster()
		{
			var cluster = _cluster;

			if (cluster != null)
			{
				cluster.Dispose();
			}
		}


		public void Dispose()
		{
			ShutdownCluster();
		}
	}
}