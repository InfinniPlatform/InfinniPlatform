using System;

using InfinniPlatform.UserInterface.Dynamic;
using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewDesigner
{
	/// <summary>
	/// Элемент представления для редактирования представлений.
	/// </summary>
	sealed class ViewDesignerElement : BaseElement<ViewDesignerControl>
	{
		public ViewDesignerElement(View view)
			: base(view)
		{
			Control.OnValueChanged += OnValueChangedHandler;
		}

		private void OnValueChangedHandler(object sender, EventArgs eventArgs)
		{
			this.InvokeScript(OnValueChanged, arguments =>
											  {
												  arguments.Value = GetValue();
											  });
		}


		// ConfigId

		/// <summary>
		/// Возвращает идентификатор конфигурации.
		/// </summary>
		public Func<string> GetConfigId()
		{
			return Control.ConfigId;
		}

		/// <summary>
		/// Устанавливает идентификатор конфигурации.
		/// </summary>
		public void SetConfigId(Func<string> value)
		{
			Control.ConfigId = value;
		}


		// DocumentId

		/// <summary>
		/// Возвращает идентификатор документа.
		/// </summary>
		public Func<string> GetDocumentId()
		{
			return Control.DocumentId;
		}

		/// <summary>
		/// Устанавливает идентификатор документа.
		/// </summary>
		public void SetDocumentId(Func<string> value)
		{
			Control.DocumentId = value;
		}


		// Value

		/// <summary>
		/// Возвращает значение.
		/// </summary>
		public object GetValue()
		{
			return Control.Value.JsonToObject();
		}

		/// <summary>
		/// Устанавливает значение.
		/// </summary>
		public void SetValue(object value)
		{
			Control.InvokeControl(() =>
								  {
									  Control.Value = value.ObjectToJson();
								  });
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события изменения значения.
		/// </summary>
		public ScriptDelegate OnValueChanged { get; set; }
	}
}