using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// ��������� �������.
    /// </summary>
    public interface IHttpRequestHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        /// <summary>
        /// ������ ������ ���������.
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        /// ������ �������� ���������.
        /// </summary>
        IEnumerable<IEnumerable<string>> Values { get; }

        /// <summary>
        /// ���������� �������� ��������� �� �����.
        /// </summary>
        IEnumerable<string> this[string key] { get; }


        /// <summary>
        /// ��� ����������� ���� �������.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// ������ ���� �������.
        /// </summary>
        long ContentLength { get; }
    }
}