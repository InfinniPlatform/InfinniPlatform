using System;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Именованный элемент.
    /// </summary>
    [Serializable]
    public abstract class PrintNamedItem
    {
        /// <summary>
        /// Имя элемента.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public abstract string GetDisplayTypeName();

        /// <summary>
        /// Возвращает строковое представление элемента.
        /// </summary>
        public virtual string GetDisplayName()
        {
            var result = GetDisplayTypeName();

            if (!string.IsNullOrEmpty(Name))
            {
                result += ": " + Name;
            }

            return result;
        }
    }
}