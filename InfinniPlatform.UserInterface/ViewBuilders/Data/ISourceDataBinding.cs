using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data
{
	/// <summary>
	/// Привязка источника данных к элементу представления.
	/// </summary>
	public interface ISourceDataBinding : IViewChild
	{
		/// <summary>
		/// Возвращает наименование источника данных.
		/// </summary>
		string GetDataSource();

		/// <summary>
		/// Возвращает путь к свойству источника данных.
		/// </summary>
		string GetProperty();

		/// <summary>
		/// Устанавливает значение у элемента представления.
		/// </summary>
		/// <remarks>
		/// Вызывает источник данных для оповещения элемента представления об изменениях.
		/// </remarks>
		void PropertyValueChanged(object value, bool force = false);

		/// <summary>
		/// Возвращает или устанавливает обработчик события изменения значения в элементе представления.
		/// </summary>
		/// <remarks>
		/// Обработчик устанавливает источник данных для получения уведомлений об изменениях значения в элементе представления.
		/// </remarks>
		ScriptDelegate OnSetPropertyValue { get; set; }
	}
}