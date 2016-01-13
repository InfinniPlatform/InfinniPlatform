using System;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// ����������� ������ ������������� ��������.
    /// </summary>
    public interface IHttpRouteBuilder
    {
        /// <summary>
        /// ������������� ���������� ��������.
        /// </summary>
        /// <param name="path">���� �������.</param>
        /// <returns>���������� �������.</returns>
        Func<IHttpRequest, IHttpResponse> this[string path] { set; }
    }
}