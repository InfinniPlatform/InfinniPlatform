using System.Linq;

using InfinniPlatform.Core.Index;
using InfinniPlatform.ElasticSearch.Factories;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    /// <summary>
    /// Модуль для получения документа по указанному идентификатору
    /// Особенность: при выполнении запроса по идентификатору НЕ выполняется проверка прав (неизвестен индекс и тип получаемого
    /// документа)
    /// </summary>
    public sealed class ActionUnitGetDocumentById
    {
        public ActionUnitGetDocumentById(IReferenceResolver referenceResolver,
                                         IIndexFactory indexFactory)
        {
            _referenceResolver = referenceResolver;
            _indexFactory = indexFactory;
        }

        private readonly IIndexFactory _indexFactory;
        private readonly IReferenceResolver _referenceResolver;

        public void Action(IApplyContext target)
        {
            var documentProvider = _indexFactory.BuildAllIndexesOperationProvider();

            if (string.IsNullOrEmpty(target.Item.ConfigId) ||
                string.IsNullOrEmpty(target.Item.DocumentId))
            {
                target.Result = documentProvider.GetItem(target.Item.Id);
            }
            else
            {
                var docsToResolve = new[] { documentProvider.GetItem(target.Item.Id) };
                _referenceResolver.ResolveReferences(target.Item.ConfigId, target.Item.DocumentId, docsToResolve, null);

                target.Result = docsToResolve.FirstOrDefault();
            }
        }
    }
}