using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware
{
    /// <summary>
    ///  Форматирование роутингов запросов
    /// </summary>
    public interface IRouteFormatter
    {
        /// <summary>
        ///   Заполнить список параметров роутинга запросов
        /// </summary>
        /// <param name="context">Контекст роутинга</param>
        /// <returns></returns>
        Dictionary<string, string> GetRouteDictionary(IOwinContext context);

        /// <summary>
        ///   Сформировать результирующий URL запроса, используя указанные параметры
        /// </summary>
        /// <param name="context">Контекст выполнения запроса</param>
        /// <param name="path">Шаблон запроса</param>
        /// <returns>Сформированный URL запроса</returns>
        PathString FormatRoutePath(IOwinContext context, PathString path);
    }
}
