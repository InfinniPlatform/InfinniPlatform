using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.SystemConfig.Utils;

namespace InfinniPlatform.SystemConfig.ActionUnits
{
    /// <summary>
    /// Модуль для получения документа по указанному идентификатору
    /// Особенность: при выполнении запроса по идентификатору НЕ выполняется проверка прав (неизвестен индекс и тип получаемого
    /// документа)
    /// </summary>
    public sealed class ActionUnitGetDocumentById
    {
        public ActionUnitGetDocumentById(DocumentExecutor documentExecutor)
        {
            _documentExecutor = documentExecutor;
        }

        private readonly DocumentExecutor _documentExecutor;

        public void Action(IApplyContext target)
        {
            if (string.IsNullOrEmpty(target.Item.ConfigId) ||
                string.IsNullOrEmpty(target.Item.DocumentId))
            {
                target.Result = _documentExecutor.GetBaseDocument(target.Item.Id);
            }
            else
            {
                target.Result = _documentExecutor.GetCompleteDocument(target.Item.ConfigId,
                                                                      target.Item.DocumentId, target.Item.Id);
            }
        }
    }
}