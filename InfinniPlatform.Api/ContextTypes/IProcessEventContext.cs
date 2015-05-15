using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Events;

namespace InfinniPlatform.Api.ContextTypes
{
    /// <summary>
    ///   Контекст обработчика применения событий
    /// </summary>
    public interface IProcessEventContext : ICommonContext
    {
        /// <summary>
        ///   Идентификатор обрабатываемого объекта
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///   Список событий
        /// </summary>
        List<EventDefinition> Events { get; set; }

        /// <summary>
        ///   Свойства, значения которых должны устанавливаться по умолчанию
        /// </summary>
        List<EventDefinition> DefaultProperties { get; set; }

        /// <summary>
        ///   Результат обработки событий
        /// </summary>
        dynamic Item { get; set; }
    }
}
