using DevExpress.Xpf.Editors;

using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CheckBox
{
	/// <summary>
	/// Элемент представления для отображения и редактирования логического значения в виде флажка.
	/// </summary>
	public sealed class CheckBoxElement : BaseElement<CheckEdit>
	{
		public CheckBoxElement(View view)
			: base(view)
		{
			Control.Height = 22;
			Control.EditValueChanged += OnEditValueChanged;
		}

		private void OnEditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			_value = e.NewValue;

			this.InvokeScript(OnValueChanged, args =>
											  {
												  args.Value = e.NewValue;
											  });
		}


		// Text

		public override void SetText(string value)
		{
			base.SetText(value);

			Control.InvokeControl(() =>
								  {
									  Control.Content = value;
								  });
		}


		// ReadOnly

		private bool _readOnly;

		/// <summary>
		/// Возвращает значение, определяющее, запрещено ли редактирование значения.
		/// </summary>
		public bool GetReadOnly()
		{
			return _readOnly;
		}

		/// <summary>
		/// Устанавливает значение, определяющее, запрещено ли редактирование значения.
		/// </summary>
		public void SetReadOnly(bool value)
		{
			if (_readOnly != value)
			{
				_readOnly = value;

				Control.IsReadOnly = value;
			}
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

			Control.InvokeControl(() =>
								  {
									  Control.EditValue = value;
								  });
		}


		// Events

		/// <summary>
		/// Возвращает или устанавливает обработчик события изменения значения.
		/// </summary>
		public ScriptDelegate OnValueChanged { get; set; }
	}
}