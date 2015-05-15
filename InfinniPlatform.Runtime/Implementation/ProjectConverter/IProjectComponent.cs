using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    interface IProjectComponent
    {
        /// <summary>
        /// Возвращает список существенных аттрибутов для данного IProjectComponent
        /// </summary>
        IEnumerable<ReflectionPair> GetAttributes();

        /// <summary>
        /// Возвращает список существенных дочерних эелементов для данного IProjectComponent
        /// </summary>
        IEnumerable<ReflectionPair> GetElements();
    }
}
