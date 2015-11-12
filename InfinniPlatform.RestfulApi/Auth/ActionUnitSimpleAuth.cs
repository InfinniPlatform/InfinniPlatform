using System;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    /// Модуль проверки авторизации
    /// </summary>
    [Obsolete]
    public sealed class ActionUnitSimpleAuth
    {
        public void Action(IApplyContext target)
        {
            // TODO: Без проверки прав пользователя, от имени которого выполняется данный запрос, выполнять эти действия нельзя!
            // На текущий момент нет адекватного общего механизма для выполнения подобной проверки.

            target.Result = new DynamicWrapper();
            target.Result.IsValid = false;
            target.Result.ValidationMessage = "Not Supported";
        }
    }
}