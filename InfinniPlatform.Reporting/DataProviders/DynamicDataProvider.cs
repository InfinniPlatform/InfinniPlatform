using System.Collections;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Reporting.DataProviders
{
	/// <summary>
	/// Провайдер для доступа к данным динамических объектов.
	/// </summary>
	sealed class DynamicDataProvider : IDataProvider
	{
		public DynamicDataProvider(IEnumerable dynamicArray)
		{
			_dynamicArray = dynamicArray ?? new dynamic[] { };
		}


		private readonly IEnumerable _dynamicArray;


		public object GetPropertyValue(object instance, string propertyName)
		{
			object propertyValue;

			if (TryGetValue(instance, propertyName, out propertyValue) == false
				&& JsonDataProvider.TryGetValue(instance, propertyName, out propertyValue) == false
				&& KeyValueDataProvider.TryGetValue(instance, propertyName, out propertyValue) == false
				&& ObjectDataProvider.TryGetValue(instance, propertyName, out propertyValue) == false)
			{
			}

			return propertyValue;
		}

		public static bool TryGetValue(object instance, string propertyName, out object propertyValue)
		{
			var success = false;

			propertyValue = null;

			var dynamicObject = instance as DynamicWrapper;

			if (dynamicObject != null)
			{
				propertyValue = dynamicObject[propertyName];
				success = true;
			}

			return success;
		}


		public IEnumerator GetEnumerator()
		{
			return _dynamicArray.GetEnumerator();
		}
	}
}
