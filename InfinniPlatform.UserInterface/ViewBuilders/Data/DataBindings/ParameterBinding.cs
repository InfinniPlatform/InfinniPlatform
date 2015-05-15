﻿using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataBindings
{
	/// <summary>
	/// Описывает связь между элементом представления и параметром представления.
	/// </summary>
	public sealed class ParameterBinding : IElementDataBinding, ISourceDataBinding
	{
		public ParameterBinding(View view, string dataSource, string property)
		{
			_view = view;
			_dataSource = dataSource;
			_property = property;
		}


		// View

		private readonly View _view;

		public View GetView()
		{
			return _view;
		}


		// DataSource

		private readonly string _dataSource;

		public string GetDataSource()
		{
			return _dataSource;
		}


		// Property

		private readonly string _property;

		public string GetProperty()
		{
			return _property;
		}


		// Binding

		private object _value;

		public void SetPropertyValue(object value, bool force = false)
		{
			if (force || !Equals(_value, value))
			{
				_value = value;

				InvokePropertyValueEventHandler(OnSetPropertyValue, force);
			}
		}

		public void PropertyValueChanged(object value, bool force = false)
		{
			if (force || !Equals(_value, value))
			{
				_value = value;

				InvokePropertyValueEventHandler(OnPropertyValueChanged, force);
			}
		}

		public ScriptDelegate OnSetPropertyValue { get; set; }

		public ScriptDelegate OnPropertyValueChanged { get; set; }


		private void InvokePropertyValueEventHandler(ScriptDelegate handler, bool force)
		{
			this.InvokeScript(handler, args =>
									   {
										   args.DataSource = _dataSource;
										   args.Property = _property;
										   args.Value = _value;
										   args.Force = force;
									   });
		}
	}
}