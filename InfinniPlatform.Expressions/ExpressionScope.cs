using System.Collections.Generic;

namespace InfinniPlatform.Expressions
{
    /// <summary>
    ///     Контекст исполнения выражения.
    /// </summary>
    public sealed class ExpressionScope
    {
        private readonly ExpressionScope _parent;
        private readonly Dictionary<string, object> _variables;

        /// <summary>
        ///     Конструктор.
        /// </summary>
        /// <param name="parent">Родительский контекст исполнения.</param>
        public ExpressionScope(ExpressionScope parent = null)
        {
            _parent = parent;
            _variables = new Dictionary<string, object>();
        }

        /// <summary>
        ///     Объявляет переменную в текущем контексте.
        /// </summary>
        /// <param name="name">Наименование переменной.</param>
        /// <param name="value">Значение переменной.</param>
        public void DeclareVariable(string name, object value = null)
        {
            _variables[name] = value;
        }

        /// <summary>
        ///     Устанавливает значение переменной.
        /// </summary>
        /// <param name="name">Наименование переменной.</param>
        /// <param name="value">Значение переменной.</param>
        public void SetVariable(string name, object value)
        {
            var parent = this;

            // Поиск контекста, в котором объявлена переменная
            while (parent != null)
            {
                if (parent._variables.ContainsKey(name))
                {
                    parent._variables[name] = value;
                    break;
                }

                parent = parent._parent;
            }

            // Если переменная не объявлена, она объявляется в локальном контексте
            if (parent == null)
            {
                _variables[name] = value;
            }
        }

        /// <summary>
        ///     Возвращает значение переменной.
        /// </summary>
        /// <param name="name">Наименование переменной.</param>
        /// <returns>Значение переменной.</returns>
        public object GetVariable(string name)
        {
            object value = null;

            var parent = this;

            // Поиск контекста, в котором объявлена переменная
            while (parent != null)
            {
                if (parent._variables.TryGetValue(name, out value))
                {
                    break;
                }

                parent = parent._parent;
            }

            return value;
        }
    }
}