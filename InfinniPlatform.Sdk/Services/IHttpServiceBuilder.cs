using System;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// ����������� ������������ �������� �������.
    /// </summary>
    public interface IHttpServiceBuilder
    {
        /// <summary>
        /// ������� ��������� GET-��������.
        /// </summary>
        IHttpRouteBuilder Get { get; }

        /// <summary>
        /// ������� ��������� POST-��������.
        /// </summary>
        IHttpRouteBuilder Post { get; }

        /// <summary>
        /// ������� ��������� PUT-��������.
        /// </summary>
        IHttpRouteBuilder Put { get; }

        /// <summary>
        /// ������� ��������� PATCH-��������.
        /// </summary>
        IHttpRouteBuilder Patch { get; }

        /// <summary>
        /// ������� ��������� DELETE-��������.
        /// </summary>
        IHttpRouteBuilder Delete { get; }

        /// <summary>
        /// �������������� ��������.
        /// </summary>
        Action<IHttpRequest> OnBefore { get; set; }

        /// <summary>
        /// �������������� ��������.
        /// </summary>
        Action<IHttpRequest, IHttpResponse> OnAfter { get; set; }
    }
}