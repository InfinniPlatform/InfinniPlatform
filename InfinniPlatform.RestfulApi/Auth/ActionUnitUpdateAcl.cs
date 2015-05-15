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
    ///   Модуль обновления ACL в кэше сервера
    /// </summary>
    public sealed class ActionUnitUpdateAcl
    {
        public void Action(IApplyContext target)
        {
			target.Context.GetComponent<SecurityComponent>().UpdateAcl();
        }
    }
}
