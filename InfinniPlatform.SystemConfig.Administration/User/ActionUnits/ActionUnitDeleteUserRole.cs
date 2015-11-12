using System;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    [Obsolete]
    public sealed class ActionUnitDeleteUserRole
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