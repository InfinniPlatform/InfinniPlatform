using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.ContextComponents;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///   Модуль вызова обновления данных о ролях пользователей в кэше
    /// </summary>
    public sealed class ActionUnitUpdateUserRoles
    {
        public void Action(IApplyContext applyContext)
        {
            applyContext.Context.GetComponent<SecurityComponent>().UpdateRoles();
        }
    }
}
