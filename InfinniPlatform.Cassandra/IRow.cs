using System;
using System.Collections.Generic;

namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Represents row.
	/// </summary>
	public interface IRow : IEnumerable<object>
	{
		T GetValue<T>(int index);
		T GetValue<T>(string name);

		object GetValue(int index);
		object GetValue(string name);

		object GetValue(Type type, int index);
		object GetValue(Type type, string name);
	}
}