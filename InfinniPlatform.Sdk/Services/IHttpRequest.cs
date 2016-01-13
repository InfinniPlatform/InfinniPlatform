using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// ������.
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// ����� �������.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// ���� �������.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// ��������� �������.
        /// </summary>
        IHttpRequestHeaders Headers { get; }

        /// <summary>
        /// ��������� ������ �������.
        /// </summary>
        dynamic Parameters { get; }

        /// <summary>
        /// ������ ������ �������.
        /// </summary>
        dynamic Query { get; }

        /// <summary>
        /// ������ ���� �������.
        /// </summary>
        dynamic Form { get; }

        /// <summary>
        /// ������ ������ �������.
        /// </summary>
        IEnumerable<IHttpRequestFile> Files { get; }

        /// <summary>
        /// ���������� ���� �������.
        /// </summary>
        Stream Content { get; }
    }
}