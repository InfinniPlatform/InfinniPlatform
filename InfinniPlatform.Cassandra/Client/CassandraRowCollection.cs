using System.Collections;
using System.Collections.Generic;

using Cassandra;

namespace InfinniPlatform.Cassandra.Client
{
	/// <summary>
	/// Represents wrapper for <see cref="RowSet"/>.
	/// </summary>
	sealed class CassandraRowCollection : IEnumerable<IRow>
	{
		public CassandraRowCollection(RowSet cassandraRowSet)
		{
			_cassandraRowSet = cassandraRowSet;
		}


		private readonly RowSet _cassandraRowSet;


		private CassandraRowEnumerator _cassandraRowEnumerator;

		private CassandraRowEnumerator GetCassandraRowEnumerator()
		{
			if (_cassandraRowEnumerator == null)
			{
				var cassandraRows = _cassandraRowSet.GetRows();

				_cassandraRowEnumerator = new CassandraRowEnumerator(cassandraRows);
			}

			return _cassandraRowEnumerator;
		}


		IEnumerator<IRow> IEnumerable<IRow>.GetEnumerator()
		{
			return GetCassandraRowEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetCassandraRowEnumerator();
		}
	}
}