using System.IO;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// ���������� � ����� �������.
    /// </summary>
    public interface IHttpRequestFile
    {
        /// <summary>
        /// ��� ����������� �����.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// ������������ �����.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ������� �����.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// ����� � ������.
        /// </summary>
        Stream Value { get; }
    }
}