using System;

using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    /// Модуль авторизации стандартного процесса обработки документов
    /// </summary>
    [Obsolete]
    public sealed class ActionUnitDocumentAuth
    {
        public void Action(IApplyContext target)
        {
            target.IsValid = true;
        }
    }
}