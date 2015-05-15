using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.Reporting.DataProviders
{
	/// <summary>
	/// Провайдер для доступа к данным типа ключ-значение.
	/// </summary>
	sealed class KeyValueDataProvider : IDataProvider
	{
		public KeyValueDataProvider(IEnumerable<IDictionary<string, object>> dataRecords)
		{
			_dataRecords = dataRecords ?? new IDictionary<string, object>[] { };
		}


		private readonly IEnumerable<IDictionary<string, object>> _dataRecords;


		public object GetPropertyValue(object instance, string propertyName)
		{
			object propertyValue;
			TryGetValue(instance, propertyName, out propertyValue);

			return propertyValue;
		}

		public static bool TryGetValue(object instance, string propertyName, out object propertyValue)
		{
			var success = false;

			propertyValue = null;

			var record = instance as IDictionary<string, object>;

			if (record != null)
			{
				success = record.TryGetValue(propertyName, out propertyValue);
			}

			return success;
		}


		public IEnumerator GetEnumerator()
		{
			return _dataRecords.GetEnumerator();
		}
	}
}