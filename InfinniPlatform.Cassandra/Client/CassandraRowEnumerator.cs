using System.Collections;
using System.Collections.Generic;

using Cassandra;

namespace InfinniPlatform.Cassandra.Client
{
	/// <summary>
	/// Represents wrapper for the collection of <see cref="Row"/>.
	/// </summary>
	sealed class CassandraRowEnumerator : IEnumerator<IRow>
	{
		public CassandraRowEnumerator(IEnumerable<Row> cassandraRows)
		{
			_cassandraRowEnumerator = cassandraRows.GetEnumerator();
		}


		private readonly IEnumerator<Row> _cassandraRowEnumerator;


		public IRow Current
		{
			get { return new CassandraRow(_cassandraRowEnumerator.Current); }
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}

		public bool MoveNext()
		{
			return _cassandraRowEnumerator.MoveNext();
		}

		public void Reset()
		{
			_cassandraRowEnumerator.Reset();
		}

		public void Dispose()
		{
			_cassandraRowEnumerator.Dispose();
		}
	}
}