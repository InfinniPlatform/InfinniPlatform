using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Api.LocalRouting
{
    /// <summary>
    ///     Контракт вызова методов локальных запросов
    /// </summary>
    public interface IRequestLocal
    {
        string InvokeRestOperationPost(string version, string configuration, string metadata, string action,
            IDictionary<string, object> requestBody, string userName);

        string InvokeRestOperationUpload(string version, string configuration, string metadata, string action,
            object requestBody, string filePath, string userName);

        string InvokeRestOperationGet(string version, string configuration, string metadata, string action,
            IDictionary<string, object> requestBody, string userName);

        string InvokeRestOperationUpload(string version, string configuration, string metadata, string action,
            object requestBody, Stream file, string userName);

        string InvokeRestOperationDownload(string version, string configuration, string metadata, string action,
            object requestBody, string userName);
    }
}