using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Core.LocalRouting
{
    /// <summary>
    /// Контракт вызова методов локальных запросов
    /// </summary>
    public interface IRequestLocal
    {
        string InvokeRestOperationGet(string configuration, string metadata, string action, IDictionary<string, object> requestBody, string userName);
        string InvokeRestOperationPost(string configuration, string metadata, string action, IDictionary<string, object> requestBody, string userName);

        string InvokeRestOperationUpload(string configuration, string metadata, string action, object requestBody, string filePath, string userName);
        string InvokeRestOperationUpload(string configuration, string metadata, string action, object requestBody, Stream file, string userName);

        string InvokeRestOperationDownload(string configuration, string metadata, string action, object requestBody, string userName);
    }
}