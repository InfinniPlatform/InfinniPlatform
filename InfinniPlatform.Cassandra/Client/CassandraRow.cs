using System;
using System.Collections;
using System.Collections.Generic;

using Cassandra;

namespace InfinniPlatform.Cassandra.Client
{
	/// <summary>
	/// Represents wrapper for <see cref="Row"/>.
	/// </summary>
	sealed class CassandraRow : IRow
	{
		public CassandraRow(Row cassandraRow)
		{
			_cassandraRow = cassandraRow;
		}


		private readonly Row _cassandraRow;


		public T GetValue<T>(int index)
		{
			return _cassandraRow.GetValue<T>(index);
		}

		public T GetValue<T>(string name)
		{
			return _cassandraRow.GetValue<T>(name);
		}


		public object GetValue(int index)
		{
			return _cassandraRow[index];
		}

		public object GetValue(string name)
		{
			return _cassandraRow[name];
		}


		public object GetValue(Type type, int index)
		{
			return _cassandraRow.GetValue(type, index);
		}

		public object GetValue(Type type, string name)
		{
			return _cassandraRow.GetValue(type, name);
		}


		public IEnumerator<object> GetEnumerator()
		{
			return ((IEnumerable<object>)_cassandraRow).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _cassandraRow.GetEnumerator();
		}
	}
}