using InfinniPlatform.UserInterface.ViewBuilders.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data.DataBindings
{
    /// <summary>
    ///     Описывает связь между элементом представления и свойством источника данных.
    /// </summary>
    public sealed class PropertyBinding : IElementDataBinding, ISourceDataBinding
    {
        // Binding

        private object _value;
        // DataSource

        private readonly string _dataSource;
        // Property

        private readonly string _property;
        // View

        private readonly View _view;

        public PropertyBinding(View view, string dataSource, string property)
        {
            _view = view;
            _dataSource = dataSource;
            _property = property;
        }

        public View GetView()
        {
            return _view;
        }

        public void SetPropertyValue(object value, bool force = false)
        {
            if (force || !Equals(_value, value))
            {
                _value = value;

                InvokePropertyValueEventHandler(OnSetPropertyValue, force);
            }
        }

        public ScriptDelegate OnPropertyValueChanged { get; set; }

        public string GetDataSource()
        {
            return _dataSource;
        }

        public string GetProperty()
        {
            return _property;
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