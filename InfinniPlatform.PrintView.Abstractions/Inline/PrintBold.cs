﻿using System;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Inline
{
    /// <summary>
    /// Элемент для выделения содержимого полужирным шрифтом.
    /// </summary>
    [Serializable]
    public class PrintBold : PrintSpan
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public new const string TypeName = "Bold";


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintBold;
        }
    }
}