using System.Collections;

namespace InfinniPlatform.Reporting.DataProviders
{
	/// <summary>
	/// Провайдер для доступа к строготипизированным данным.
	/// </summary>
	sealed class ObjectDataProvider : IDataProvider
	{
		public ObjectDataProvider(IEnumerable objectArray)
		{
			_objectArray = objectArray ?? new object[] { };
		}


		private readonly IEnumerable _objectArray;


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

			if (instance != null)
			{
				var instanceType = instance.GetType();

				var propertyInfo = instanceType.GetProperty(propertyName);

				if (propertyInfo != null)
				{
					propertyValue = propertyInfo.GetValue(instance);
					success = true;
				}
				else
				{
					var fieldInfo = instanceType.GetField(propertyName);

					if (fieldInfo != null)
					{
						propertyValue = fieldInfo.GetValue(instance);
						success = true;
					}
				}
			}

			return success;
		}


		public IEnumerator GetEnumerator()
		{
			return _objectArray.GetEnumerator();
		}
	}
}