using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Schema;

namespace InfinniPlatform.Core.Index.SearchOptions
{
    /// <summary>
    ///     Анализатор критериев запроса данных
    /// </summary>
    public sealed class QueryCriteriaAnalyzer
    {
        private readonly List<SchemaObject> _resolveProperties = new List<SchemaObject>();

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="metadataComponent">Провайдер метаданных сервисной части</param>
        /// <param name="schema">Схема данных документа, к которому выполняется запрос</param>
        public QueryCriteriaAnalyzer(IMetadataComponent metadataComponent, dynamic schema)
        {
            var metadataIterator = new SchemaIterator(new SchemaProvider(metadataComponent));
            metadataIterator.OnObjectProperty = schemaObject =>
            {
                if (schemaObject.IsResolve)
                {
                    _resolveProperties.Add(schemaObject);
                }
            };
            metadataIterator.OnArrayProperty = schemaObject =>
            {
                if (schemaObject.IsDocumentArray)
                {
                    _resolveProperties.Add(schemaObject);
                }
            };
            metadataIterator.ProcessSchema(schema);
        }

        /// <summary>
        ///     Получить список критериев документа, применяемых до выполнения операции Resolve
        /// </summary>
        /// <param name="filter">Список критериев запроса</param>
        /// <returns>Список критериев</returns>
        public IEnumerable<dynamic> GetBeforeResolveCriteriaList(IEnumerable<dynamic> filter)
        {
            var result = new List<dynamic>();
            foreach (var criteria in filter)
            {
                if (_resolveProperties.Any(r => ((string) criteria.Property).StartsWith(r.GetFullPath())))
                {
                    continue;
                }
                result.Add(criteria);
            }
            return result;
        }

        /// <summary>
        ///     Получить список критериев запроса, выполняемых после операции Resolve
        /// </summary>
        /// <param name="filter">Список критериев запроса</param>
        /// <returns>Список критериев</returns>
        public IEnumerable<dynamic> GetAfterResolveCriteriaList(IEnumerable<dynamic> filter)
        {
            var filtersArray = filter.ToArray();

            return filtersArray.Except(GetBeforeResolveCriteriaList(filtersArray));
        }
    }
}