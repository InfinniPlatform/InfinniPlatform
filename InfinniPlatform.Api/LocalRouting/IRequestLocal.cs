using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.LocalRouting
{
    /// <summary>
    ///   Контракт вызова методов локальных запросов
    /// </summary>
    public interface IRequestLocal
    {
		string InvokeRestOperationPost(string configuration, string metadata, string action, IDictionary<string, object> requestBody, string userName);

		string InvokeRestOperation(string configuration, string metadata, string action, object requestBody, string filePath, string userName);
        string InvokeRestOperationGet(string configuration, string metadata, string action, IDictionary<string, object> requestBody, string userName);

    }
}
