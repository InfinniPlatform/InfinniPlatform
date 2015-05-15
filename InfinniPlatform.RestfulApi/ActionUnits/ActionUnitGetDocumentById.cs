using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    /// <summary>
    ///   Модуль для получения документа по указанному идентификатору
    ///   Особенность: при выполнении запроса по идентификатору НЕ выполняется проверка прав (неизвестен индекс и тип получаемого документа)
    /// </summary>
    public sealed class ActionUnitGetDocumentById
    {
        public void Action(IApplyContext target)
        {

            var executor = new DocumentExecutor(target.Context.GetComponent<IConfigurationMediatorComponent>(),
                                                target.Context.GetComponent<IMetadataComponent>(),
                                                target.Context.GetComponent<InprocessDocumentComponent>(),
                                                target.Context.GetComponent<IProfilerComponent>());

            if (string.IsNullOrEmpty(target.Item.ConfigId) || string.IsNullOrEmpty(target.Item.DocumentId))
            {
                target.Result = executor.GetBaseDocument(target.UserName, target.Item.Id);
            }
            else
            {
                target.Result = executor.GetCompleteDocument(target.Item.ConfigId, target.Item.DocumentId,
                    target.UserName, target.Item.Id);
            }
        }
    }
}
