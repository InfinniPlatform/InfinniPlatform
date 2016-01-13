using System;
using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// �����.
    /// </summary>
    public class HttpResponse : IHttpResponse
    {
        public HttpResponse()
        {
            StatusCode = 200;
            ContentType = HttpConstants.JsonContentType;
        }

        /// <summary>
        /// ��� ���������.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// �������� ���������.
        /// </summary>
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// ��������� ������.
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// ��� ����������� ���� ������.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// ���������� ���� ������.
        /// </summary>
        public Action<Stream> Content { get; set; }
    }
}