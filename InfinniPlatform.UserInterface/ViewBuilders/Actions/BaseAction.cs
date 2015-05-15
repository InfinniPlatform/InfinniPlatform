using System;

using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
	/// <summary>
	/// Действие для вызова из визуального представления.
	/// </summary>
	public class BaseAction : IViewChild
	{
		public BaseAction(View view)
		{
			_view = view;
		}


		// View

		private readonly View _view;

		public View GetView()
		{
			return _view;
		}


		// Action

		private Action _action;

		/// <summary>
		/// Возвращает функцию для выполнения действия.
		/// </summary>
		public Action GetAction()
		{
			return _action;
		}

		/// <summary>
		/// Устанавливает функцию для выполнения действия.
		/// </summary>
		public void SetAction(Action value)
		{
			_action = value;
		}

		/// <summary>
		/// Вызывает функцию для выполнения действия.
		/// </summary>
		public void Execute()
		{
			if (_action != null)
			{
				_action();
			}
		}
	}
}