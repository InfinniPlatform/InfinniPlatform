using System;
using System.Collections.Generic;

using Cassandra;

using ICassandraSession = Cassandra.ISession;

namespace InfinniPlatform.Cassandra.Client
{
	/// <summary>
	/// Represents session for the Apache Cassandra.
	/// </summary>
	sealed class CassandraSession : ISession
	{
		public CassandraSession(ICassandraSession session)
		{
			if (session == null)
			{
				throw new ArgumentNullException();
			}

			_session = session;
		}


		private readonly ICassandraSession _session;


		private static readonly object[] NullParameters = { };

		private static readonly CassandraCqlQueryStatementConverter CqlConverter
			= new CassandraCqlQueryStatementConverter();

		private static readonly Dictionary<Consistency, ConsistencyLevel> ConsistencyLevels
			= new Dictionary<Consistency, ConsistencyLevel>
				  {
					  { Consistency.Any, ConsistencyLevel.Any },
					  { Consistency.One, ConsistencyLevel.One },
					  { Consistency.Two, ConsistencyLevel.Two },
					  { Consistency.Three, ConsistencyLevel.Three },
					  { Consistency.Quorum, ConsistencyLevel.Quorum },
					  { Consistency.LocalQuorum, ConsistencyLevel.LocalQuorum },
					  { Consistency.EachQuorum, ConsistencyLevel.EachQuorum },
					  { Consistency.All, ConsistencyLevel.All }
				  };


		private readonly Dictionary<IQueryStatement, PreparedStatement> _preparedStatements
			= new Dictionary<IQueryStatement, PreparedStatement>();

		private PreparedStatement GetPreparedStatement(IQueryStatement statement)
		{
			PreparedStatement preparedStatement;

			if (_preparedStatements.TryGetValue(statement, out preparedStatement) == false)
			{
				var cqlQuery = CqlConverter.Convert(statement);

				preparedStatement = _session.Prepare(cqlQuery);

				_preparedStatements[statement] = preparedStatement;
			}

			return preparedStatement;
		}


		/// <summary>
		/// Executes specified query.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		public IEnumerable<IRow> Execute(IQueryStatement statement, object[] parameters = null, Consistency consistency = Consistency.One)
		{
			var preparedStatement = GetPreparedStatement(statement);

			var boundStatement = preparedStatement.Bind(parameters ?? NullParameters);
			boundStatement.SetConsistencyLevel(ConsistencyLevels[consistency]);

			var rowSet = _session.Execute(boundStatement);

			return new CassandraRowCollection(rowSet);
		}

		public void Dispose()
		{
			_session.Dispose();
			_preparedStatements.Clear();
		}
	}
}