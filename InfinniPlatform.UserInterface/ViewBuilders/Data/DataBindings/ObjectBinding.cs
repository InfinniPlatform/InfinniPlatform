using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataBindings
{
	/// <summary>
	/// Описывает связь между элементом представления и произвольным объектом.
	/// </summary>
	public sealed class ObjectBinding : IElementDataBinding
	{
		public ObjectBinding(View view, object value)
		{
			_view = view;
			_value = value;
		}


		// View

		private readonly View _view;

		public View GetView()
		{
			return _view;
		}


		// SetPropertyValue

		private object _value;

		public void SetPropertyValue(object value, bool force = false)
		{
			if (force || !Equals(_value, value))
			{
				_value = value;

				RaiseOnPropertyValueChanged(force);
			}
		}


		// OnPropertyValueChanged

		private ScriptDelegate _onPropertyValueChanged;

		public ScriptDelegate OnPropertyValueChanged
		{
			get
			{
				return _onPropertyValueChanged;
			}
			set
			{
				_onPropertyValueChanged = value;

				// Подписчик оповещается сразу, поскольку значение уже известно

				RaiseOnPropertyValueChanged(false);
			}
		}


		private void RaiseOnPropertyValueChanged(bool force)
		{
			this.InvokeScript(OnPropertyValueChanged, args =>
													  {
														  args.Value = _value;
														  args.Force = force;
													  });
		}
	}
}