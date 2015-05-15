using System.Collections;

namespace InfinniPlatform.Reporting.DataProviders
{
	/// <summary>
	/// Провайдер для доступа к данным.
	/// </summary>
	interface IDataProvider : IEnumerable
	{
		/// <summary>
		/// Получить значение свойства элемена источника данных.
		/// </summary>
		/// <param name="instance">Экземпляр элемента источника данных.</param>
		/// <param name="propertyName">Наименование свойства элемента источника данных.</param>
		/// <returns>Значение свойства элемента источника данных.</returns>
		object GetPropertyValue(object instance, string propertyName);
	}
}