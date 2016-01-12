using System;

namespace InfinniPlatform.Core.Sql
{
    /// <summary>
    /// Объект SQL-запроса.
    /// </summary>
    public sealed class SqlQueryObject
    {
        public SqlQueryObject(string text, object @params = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            Text = text;
            Params = @params;
        }

        /// <summary>
        /// Текст запроса.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Список параметров.
        /// </summary>
        public object Params { get; private set; }
    }
}