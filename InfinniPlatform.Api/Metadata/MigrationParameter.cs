﻿using System.Collections.Generic;

namespace InfinniPlatform.Api.Metadata
{
    /// <summary>
    /// Представляет входной параметр, который может быть передан миграции или верификации
    /// </summary>
    public class MigrationParameter
    {
        /// <summary>
        /// Заголовок параметра
        /// </summary>
        public string Caption
        {
            get;
            set;
        }

        /// <summary>
        /// Первональное значение входного параметра
        /// </summary>
        public object InitialValue
        {
            get;
            set;
        }

        /// <summary>
        /// Возможные значение параметра
        /// </summary>
        public IEnumerable<object> PossibleValues
        {
            get;
            set;
        }       
    }
}




