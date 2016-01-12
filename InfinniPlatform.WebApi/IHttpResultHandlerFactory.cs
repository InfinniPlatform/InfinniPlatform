using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.WebApi.HttpResultHandlers;

namespace InfinniPlatform.WebApi
{
    /// <summary>
    ///   Фабрика обработчиков результата запроса
    /// </summary>
    public interface IHttpResultHandlerFactory
    {
        IHttpResultHandler GetResultHandler(HttpResultHandlerType httpResultHandlerType);
    }
}
