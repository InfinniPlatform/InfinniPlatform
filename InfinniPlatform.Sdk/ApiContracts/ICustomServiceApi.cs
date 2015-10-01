using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.ApiContracts
{
    public interface ICustomServiceApi
    {
        /// <summary>
        ///   Выполнить вызов пользовательского сервиса
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        dynamic ExecuteAction(string application, string documentType, string service, dynamic body);
    }
}
