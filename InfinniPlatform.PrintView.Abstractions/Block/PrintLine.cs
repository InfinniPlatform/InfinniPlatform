﻿using System;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Block
{
    /// <summary>
    /// Элемент для создания горизонтальной линии.
    /// </summary>
    [Serializable]
    public class PrintLine : PrintBlock
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "Line";


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintLine;
        }
    }
}