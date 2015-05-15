using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;

namespace InfinniPlatform.Metadata.Handlers
{
    /// <summary>
    ///   Обработчик применения изменений к объектам
    /// </summary>
    public interface IApplyChangesHandler
    {
        /// <summary>
        ///   Применить список событий изменения к объекту с указанным идентификатором
        ///   (К новому объекту, если идентификатор объекта не указан)
        /// </summary>
        /// <param name="id">Идентификатор изменяемого объекта</param>
        /// <param name="events">Список событий на изменение объекта</param>
        /// <returns>Результат обработки изменений</returns>
        dynamic ApplyEventsWithMetadata(string id, IEnumerable<EventDefinition> events);
    }
}
