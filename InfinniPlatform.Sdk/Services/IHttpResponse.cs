using System;
using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// �����.
    /// </summary>
    public interface IHttpResponse
    {
        /// <summary>
        /// ��� ���������.
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// �������� ���������.
        /// </summary>
        string ReasonPhrase { get; set; }

        /// <summary>
        /// ��������� ������.
        /// </summary>
        IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// ��� ����������� ���� ������.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// ���������� ���� ������.
        /// </summary>
        Action<Stream> Content { get; set; }
    }
}