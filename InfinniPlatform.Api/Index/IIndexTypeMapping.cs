using System.Collections.Generic;

namespace InfinniPlatform.Api.Index
{
    /// <summary>
    ///     Маппинг типа  индекса
    /// </summary>
    public interface IIndexTypeMapping
    {
        /// <summary>
        ///     Список полей типа
        /// </summary>
        IList<PropertyMapping> Properties { get; }
    }
}