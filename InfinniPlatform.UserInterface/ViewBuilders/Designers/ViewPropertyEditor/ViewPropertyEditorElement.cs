using System.Collections.Generic;

using InfinniPlatform.UserInterface.ViewBuilders.DataElements;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewPropertyEditor
{
	public sealed class ViewPropertyEditorElement : BaseElement<ViewPropertyEditorControl>
	{
		public ViewPropertyEditorElement(View view)
			: base(view)
		{
			Control.EditValueChanged += OnEditValueChangedHandler;
		}


		private void OnEditValueChangedHandler(object sender, ValueChangedRoutedEventArgs e)
		{
			_value = e.NewValue;

			this.InvokeScript(OnValueChanged, args => args.Value = e.NewValue);
		}


		// Editors

		private IEnumerable<PropertyEditor> _editors;

		/// <summary>
		/// Возвращает список редакторов свойств.
		/// </summary>
		public IEnumerable<PropertyEditor> GetEditors()
		{
			return _editors;
		}

		/// <summary>
		/// Устанавливает список редакторов свойств.
		/// </summary>
		public void SetEditors(IEnumerable<PropertyEditor> value)
		{
			_editors = value;

			Control.InvokeControl(() => Control.PropertyEditors = value);
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
		/// Устанавливает значение.
		/// </summary>
		public void SetValue(object value)
		{
			_value = value;

			Control.InvokeControl(() => Control.EditValue = value);
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события изменения значения.
		/// </summary>
		public ScriptDelegate OnValueChanged { get; set; }
	}
}