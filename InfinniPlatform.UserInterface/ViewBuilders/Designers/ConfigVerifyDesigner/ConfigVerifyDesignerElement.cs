using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigVerifyDesigner
{
	/// <summary>
	/// Элемент представления для проверки конфигурации.
	/// </summary>
	sealed class ConfigVerifyDesignerElement : BaseElement<ConfigVerifyDesignerControl>
	{
		public ConfigVerifyDesignerElement(View view)
			: base(view)
		{
		}


		// Value

		private object _value;

		/// <summary>
		/// Возвращает значение.
		/// </summary>
		public object GetValue()
		{
			return _value;
		}

		/// <summary>
		/// Возвращает значение.
		/// </summary>
		public void SetValue(object value)
		{
			_value = value;

			Control.InvokeControl(() =>
								  {
									  Control.Value = value;
								  });
		}
	}
}