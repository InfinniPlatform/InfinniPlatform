using System;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Actions
{
    /// <summary>
    ///     Действие для вызова из визуального представления.
    /// </summary>
    public class BaseAction : IViewChild
    {
        // Action

        private Action _action;
        // View

        private readonly View _view;

        public BaseAction(View view)
        {
            _view = view;
        }

        public View GetView()
        {
            return _view;
        }

        /// <summary>
        ///     Возвращает функцию для выполнения действия.
        /// </summary>
        public Action GetAction()
        {
            return _action;
        }

        /// <summary>
        ///     Устанавливает функцию для выполнения действия.
        /// </summary>
        public void SetAction(Action value)
        {
            _action = value;
        }

        /// <summary>
        ///     Вызывает функцию для выполнения действия.
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