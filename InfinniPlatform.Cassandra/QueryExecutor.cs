using System;
using System.Collections.Generic;

namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Represents query executor with specified lifetime policy for the session.
	/// </summary>
	sealed class QueryExecutor : IQueryExecutor, IDisposable
	{
		public QueryExecutor(ISessionFactory sessionFactory)
		{
			if (sessionFactory == null)
			{
				throw new ArgumentNullException();
			}

			_sessionFactory = sessionFactory;
		}


		private readonly ISessionFactory _sessionFactory;


		/// <summary>
		/// Executes specified query.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		public IEnumerable<IRow> Execute(IQueryStatement statement, object[] parameters = null, Consistency consistency = Consistency.One)
		{
			return GetSession().Execute(statement, parameters, consistency);
		}


		private ISession _session;

		private ISession GetSession()
		{
			return _session ?? (_session = _sessionFactory.OpenSession());
		}

		private void CloseSession()
		{
			var session = _session;

			if (session != null)
			{
				session.Dispose();
			}
		}


		public void Dispose()
		{
			CloseSession();
		}
	}
}