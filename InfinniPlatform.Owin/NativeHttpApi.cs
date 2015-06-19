using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace InfinniPlatform.Owin
{
    /// <summary>
    ///     Предоставляет доступ к "HTTP Server API".
    /// </summary>
    /// <remarks>
    ///     http://msdn.microsoft.com/en-us/library/windows/desktop/aa364510(v=VS.85).aspx
    /// </remarks>
    internal static class NativeHttpApi
    {
        /// <summary>
        ///     Возвращает список с информацией о привязках сертификатов к сетевому адресу и порту.
        /// </summary>
        public static BindCertificateInfo[] GetBindCertificates()
        {
            var result = new List<BindCertificateInfo>();

            CallHttpApi(() =>
            {
                uint index = 0;
                uint invokeResult;

                do
                {
                    // Запрос на получение записи конфигурации
                    var query = new HTTP_SERVICE_CONFIG_SSL_QUERY
                    {
                        QueryDesc = HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryNext,
                        dwToken = index
                    };

                    var queryPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (HTTP_SERVICE_CONFIG_SSL_QUERY)));
                    Marshal.StructureToPtr(query, queryPtr, false);

                    var queryResultSize = 0;
                    var queryResultPtr = IntPtr.Zero;

                    const HTTP_SERVICE_CONFIG_ID queryType = HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo;

                    try
                    {
                        // Получение записи конфигурации
                        var querySize = Marshal.SizeOf(query);
                        invokeResult = HttpQueryServiceConfiguration(IntPtr.Zero, queryType, queryPtr, querySize,
                            queryResultPtr, queryResultSize, out queryResultSize, IntPtr.Zero);

                        // Перечисление не содержит элементов
                        if (invokeResult == ERROR_NO_MORE_ITEMS)
                        {
                            break;
                        }

                        // Недостаточный размер буфера для помещения результата запроса
                        if (invokeResult == ERROR_INSUFFICIENT_BUFFER)
                        {
                            queryResultPtr = Marshal.AllocCoTaskMem(queryResultSize);

                            try
                            {
                                // Получение записи конфигурации (вторая попытка)
                                invokeResult = HttpQueryServiceConfiguration(IntPtr.Zero, queryType, queryPtr, querySize,
                                    queryResultPtr, queryResultSize, out queryResultSize, IntPtr.Zero);
                                ThrowWin32ExceptionIfError(invokeResult);

                                // Интерпретация полученного результата запроса из указателя
                                var queryResult =
                                    (HTTP_SERVICE_CONFIG_SSL_SET)
                                        Marshal.PtrToStructure(queryResultPtr, typeof (HTTP_SERVICE_CONFIG_SSL_SET));

                                var certificate = new byte[queryResult.ParamDesc.SslHashLength];
                                Marshal.Copy(queryResult.ParamDesc.pSslHash, certificate, 0, certificate.Length);

                                StoreName certificateStore;
                                Enum.TryParse(queryResult.ParamDesc.pSslCertStoreName, out certificateStore);

                                result.Add(new BindCertificateInfo
                                {
                                    EndPoint = CreateManagedSocketAddress(queryResult.KeyDesc.pIpPort),
                                    Certificate = certificate,
                                    CertificateStore = certificateStore,
                                    ApplicationId = queryResult.ParamDesc.AppId
                                });

                                index++;
                            }
                            finally
                            {
                                Marshal.FreeCoTaskMem(queryResultPtr);
                            }
                        }
                        else
                        {
                            ThrowWin32ExceptionIfError(invokeResult);
                        }
                    }
                    finally
                    {
                        Marshal.FreeCoTaskMem(queryPtr);
                    }
                } while (invokeResult == NOERROR);
            });

            return result.ToArray();
        }

        /// <summary>
        ///     Возвращает информацию о привязке сертификата к сетевому адресу и порту.
        /// </summary>
        public static BindCertificateInfo GetBindCertificate(IPEndPoint endPoint)
        {
            BindCertificateInfo result = null;

            CallHttpApi(() =>
            {
                var socketAddressHandle = CreateNativeSocketAddress(endPoint);
                var socketAddressPtr = socketAddressHandle.AddrOfPinnedObject();

                // Запрос на получение записи конфигурации
                var query = new HTTP_SERVICE_CONFIG_SSL_QUERY
                {
                    QueryDesc = HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryExact,
                    KeyDesc = new HTTP_SERVICE_CONFIG_SSL_KEY(socketAddressPtr)
                };

                var queryPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (HTTP_SERVICE_CONFIG_SSL_QUERY)));
                Marshal.StructureToPtr(query, queryPtr, false);

                var queryResultSize = 0;
                var queryResultPtr = IntPtr.Zero;

                const HTTP_SERVICE_CONFIG_ID queryType = HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo;

                try
                {
                    // Получение записи конфигурации
                    var querySize = Marshal.SizeOf(query);
                    var invokeResult = HttpQueryServiceConfiguration(IntPtr.Zero, queryType, queryPtr, querySize,
                        queryResultPtr, queryResultSize, out queryResultSize, IntPtr.Zero);

                    // По запросу ничего не найдено
                    if (invokeResult == ERROR_FILE_NOT_FOUND)
                    {
                        return;
                    }

                    // Недостаточный размер буфера для помещения результата запроса
                    if (invokeResult == ERROR_INSUFFICIENT_BUFFER)
                    {
                        queryResultPtr = Marshal.AllocCoTaskMem(queryResultSize);

                        try
                        {
                            // Получение записи конфигурации (вторая попытка)
                            invokeResult = HttpQueryServiceConfiguration(IntPtr.Zero, queryType, queryPtr, querySize,
                                queryResultPtr, queryResultSize, out queryResultSize, IntPtr.Zero);
                            ThrowWin32ExceptionIfError(invokeResult);

                            // Интерпретация полученного результата запроса из указателя
                            var queryResult =
                                (HTTP_SERVICE_CONFIG_SSL_SET)
                                    Marshal.PtrToStructure(queryResultPtr, typeof (HTTP_SERVICE_CONFIG_SSL_SET));

                            var certificate = new byte[queryResult.ParamDesc.SslHashLength];
                            Marshal.Copy(queryResult.ParamDesc.pSslHash, certificate, 0, certificate.Length);

                            StoreName certificateStore;
                            Enum.TryParse(queryResult.ParamDesc.pSslCertStoreName, out certificateStore);

                            result = new BindCertificateInfo
                            {
                                EndPoint = endPoint,
                                Certificate = certificate,
                                CertificateStore = certificateStore,
                                ApplicationId = queryResult.ParamDesc.AppId
                            };
                        }
                        finally
                        {
                            Marshal.FreeCoTaskMem(queryResultPtr);
                        }
                    }
                    else
                    {
                        ThrowWin32ExceptionIfError(invokeResult);
                    }
                }
                finally
                {
                    Marshal.FreeCoTaskMem(queryPtr);

                    if (socketAddressHandle.IsAllocated)
                    {
                        socketAddressHandle.Free();
                    }
                }
            });

            return result;
        }

        /// <summary>
        ///     Создает привязку сертификата к сетевому адресу и порту.
        /// </summary>
        public static void CreateBindCertificate(IPEndPoint endPoint, byte[] certificate, StoreName certificateStore,
            Guid applicationId)
        {
            CallHttpApi(() =>
            {
                var socketAddressHandle = CreateNativeSocketAddress(endPoint);
                var socketAddressPtr = socketAddressHandle.AddrOfPinnedObject();
                var certificateHandle = GCHandle.Alloc(certificate, GCHandleType.Pinned);

                // Запись конфигурации с информацией о привязке сертификата к сетевому адресу и порту
                var configInfo = new HTTP_SERVICE_CONFIG_SSL_SET
                {
                    KeyDesc = new HTTP_SERVICE_CONFIG_SSL_KEY(socketAddressPtr),
                    ParamDesc = new HTTP_SERVICE_CONFIG_SSL_PARAM
                    {
                        AppId = applicationId,
                        DefaultCertCheckMode = 0,
                        DefaultFlags = 0,
                        DefaultRevocationFreshnessTime = 0,
                        DefaultRevocationUrlRetrievalTimeout = 0,
                        pSslCertStoreName = certificateStore.ToString(),
                        pSslHash = certificateHandle.AddrOfPinnedObject(),
                        SslHashLength = certificate.Length
                    }
                };

                var configInfoPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (HTTP_SERVICE_CONFIG_SSL_SET)));
                Marshal.StructureToPtr(configInfo, configInfoPtr, false);

                const HTTP_SERVICE_CONFIG_ID queryType = HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo;

                try
                {
                    var invokeResult = HttpSetServiceConfiguration(IntPtr.Zero, queryType, configInfoPtr,
                        Marshal.SizeOf(configInfo), IntPtr.Zero);

                    if (invokeResult == ERROR_ALREADY_EXISTS)
                    {
                        // Удаление записи
                        invokeResult = HttpDeleteServiceConfiguration(IntPtr.Zero, queryType, configInfoPtr,
                            Marshal.SizeOf(configInfo), IntPtr.Zero);
                        ThrowWin32ExceptionIfError(invokeResult);

                        // Создание записи
                        invokeResult = HttpSetServiceConfiguration(IntPtr.Zero, queryType, configInfoPtr,
                            Marshal.SizeOf(configInfo), IntPtr.Zero);
                        ThrowWin32ExceptionIfError(invokeResult);
                    }
                    else
                    {
                        ThrowWin32ExceptionIfError(invokeResult);
                    }
                }
                finally
                {
                    Marshal.FreeCoTaskMem(configInfoPtr);

                    if (certificateHandle.IsAllocated)
                    {
                        certificateHandle.Free();
                    }

                    if (socketAddressHandle.IsAllocated)
                    {
                        socketAddressHandle.Free();
                    }
                }
            });
        }

        /// <summary>
        ///     Удаляет привязку сертификата к сетевому адресу и порту.
        /// </summary>
        public static void DeleteCertificateBinding(IPEndPoint endPoint)
        {
            CallHttpApi(() =>
            {
                var socketAddressHandle = CreateNativeSocketAddress(endPoint);
                var socketAddressPtr = socketAddressHandle.AddrOfPinnedObject();

                var configInfo = new HTTP_SERVICE_CONFIG_SSL_SET
                {
                    KeyDesc = new HTTP_SERVICE_CONFIG_SSL_KEY(socketAddressPtr)
                };

                var queryPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (HTTP_SERVICE_CONFIG_SSL_SET)));
                Marshal.StructureToPtr(configInfo, queryPtr, false);

                try
                {
                    var invokeResult = HttpDeleteServiceConfiguration(IntPtr.Zero,
                        HTTP_SERVICE_CONFIG_ID.HttpServiceConfigSSLCertInfo, queryPtr, Marshal.SizeOf(configInfo),
                        IntPtr.Zero);
                    ThrowWin32ExceptionIfError(invokeResult);
                }
                finally
                {
                    Marshal.FreeCoTaskMem(queryPtr);

                    if (socketAddressHandle.IsAllocated)
                    {
                        socketAddressHandle.Free();
                    }
                }
            });
        }

        // HELPERS

        private static void CallHttpApi(Action action)
        {
            var invokeResult = HttpInitialize(HttpApiVersion, HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
            ThrowWin32ExceptionIfError(invokeResult);

            try
            {
                action();
            }
            finally
            {
                HttpTerminate(HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
            }
        }

        private static void ThrowWin32ExceptionIfError(uint invokeResult)
        {
            if (invokeResult != NOERROR)
            {
                throw new Win32Exception(Convert.ToInt32(invokeResult));
            }
        }

        private static GCHandle CreateNativeSocketAddress(EndPoint ipEndPoint)
        {
            var socketAddress = ipEndPoint.Serialize();
            var socketAddressBytes = new byte[socketAddress.Size];
            var socketAddressHandle = GCHandle.Alloc(socketAddressBytes, GCHandleType.Pinned);

            for (var i = 0; i < socketAddress.Size; ++i)
            {
                socketAddressBytes[i] = socketAddress[i];
            }

            return socketAddressHandle;
        }

        private static IPEndPoint CreateManagedSocketAddress(IntPtr socketAddressPtr)
        {
            var addressFamily = (AddressFamily) Marshal.ReadInt16(socketAddressPtr);

            int socketAddressSize;
            IPEndPoint ipEndPointAny;

            switch (addressFamily)
            {
                case AddressFamily.InterNetwork:
                    socketAddressSize = 16;
                    ipEndPointAny = new IPEndPoint(IPAddress.Any, 0);
                    break;
                case AddressFamily.InterNetworkV6:
                    socketAddressSize = 28;
                    ipEndPointAny = new IPEndPoint(IPAddress.IPv6Any, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var socketAddressBytes = new byte[socketAddressSize];
            Marshal.Copy(socketAddressPtr, socketAddressBytes, 0, socketAddressSize);

            var socketAddress = new SocketAddress(AddressFamily.Unspecified, socketAddressSize);

            for (var i = 0; i < socketAddressSize; ++i)
            {
                socketAddress[i] = socketAddressBytes[i];
            }

            return (IPEndPoint) ipEndPointAny.Create(socketAddress);
        }

        /// <summary>
        ///     Информация о привязке сертификата к сетевому адресу и порту.
        /// </summary>
        public class BindCertificateInfo
        {
            /// <summary>
            ///     Сетевой адрес и порт.
            /// </summary>
            public IPEndPoint EndPoint { get; set; }

            /// <summary>
            ///     Отпечаток сертификата.
            /// </summary>
            public byte[] Certificate { get; set; }

            /// <summary>
            ///     Хранилище сертификата.
            /// </summary>
            public StoreName CertificateStore { get; set; }

            /// <summary>
            ///     Идентификатор приложения.
            /// </summary>
            public Guid ApplicationId { get; set; }
        }

        // NATIVE API

        // ReSharper disable All


        private static readonly HTTPAPI_VERSION HttpApiVersion = new HTTPAPI_VERSION(1, 0);


        /// <summary>
        ///     Инициализирует драйвер HTTP Server API.
        /// </summary>
        [DllImport("httpapi.dll", SetLastError = true)]
        private static extern uint HttpInitialize(HTTPAPI_VERSION version, uint flags, IntPtr pReserved);

        /// <summary>
        ///     Изменяет запись конфигурации драйвера HTTP Server API.
        /// </summary>
        [DllImport("httpapi.dll", SetLastError = true)]
        private static extern uint HttpSetServiceConfiguration(IntPtr serviceIntPtr, HTTP_SERVICE_CONFIG_ID configId,
            IntPtr pConfigInformation, int configInformationLength, IntPtr pOverlapped);

        /// <summary>
        ///     Удаляет запись конфигурации драйвера HTTP Server API.
        /// </summary>
        [DllImport("httpapi.dll", SetLastError = true)]
        private static extern uint HttpDeleteServiceConfiguration(IntPtr serviceIntPtr, HTTP_SERVICE_CONFIG_ID configId,
            IntPtr pConfigInformation, int configInformationLength, IntPtr pOverlapped);

        /// <summary>
        ///     Возвращает записи конфигурации драйвера HTTP Server API.
        /// </summary>
        [DllImport("httpapi.dll", SetLastError = true)]
        private static extern uint HttpQueryServiceConfiguration(IntPtr serviceIntPtr, HTTP_SERVICE_CONFIG_ID configId,
            IntPtr pInputConfigInfo, int inputConfigInfoLength, IntPtr pOutputConfigInfo, int outputConfigInfoLength,
            [Optional] out int pReturnLength, IntPtr pOverlapped);

        /// <summary>
        ///     Освобождает ресурсы драйвера HTTP Server API.
        /// </summary>
        [DllImport("httpapi.dll", SetLastError = true)]
        private static extern uint HttpTerminate(uint Flags, IntPtr pReserved);


        /// <summary>
        ///     Тип записи конфигурации драйвера HTTP Server API.
        /// </summary>
        private enum HTTP_SERVICE_CONFIG_ID
        {
            HttpServiceConfigIPListenList = 0,
            HttpServiceConfigSSLCertInfo,
            HttpServiceConfigUrlAclInfo,
            HttpServiceConfigMax
        }

        /// <summary>
        ///     Тип запроса получения записей конфигурации драйвера HTTP Server API.
        /// </summary>
        private enum HTTP_SERVICE_CONFIG_QUERY_TYPE
        {
            HttpServiceConfigQueryExact = 0,
            HttpServiceConfigQueryNext,
            HttpServiceConfigQueryMax
        }


        /// <summary>
        ///     Версия драйвера HTTP Server API.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct HTTPAPI_VERSION
        {
            public ushort HttpApiMajorVersion;
            public ushort HttpApiMinorVersion;

            public HTTPAPI_VERSION(ushort majorVersion, ushort minorVersion)
            {
                HttpApiMajorVersion = majorVersion;
                HttpApiMinorVersion = minorVersion;
            }
        }

        /// <summary>
        ///     Запрос получения записей о SSL-конфигурации драйвера HTTP Server API.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct HTTP_SERVICE_CONFIG_SSL_QUERY
        {
            public HTTP_SERVICE_CONFIG_QUERY_TYPE QueryDesc;
            public HTTP_SERVICE_CONFIG_SSL_KEY KeyDesc;
            public uint dwToken;
        }

        /// <summary>
        ///     Информация о SSL-сертификате.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct HTTP_SERVICE_CONFIG_SSL_SET
        {
            public HTTP_SERVICE_CONFIG_SSL_KEY KeyDesc;
            public HTTP_SERVICE_CONFIG_SSL_PARAM ParamDesc;
        }

        /// <summary>
        ///     Ключ записи о SSL-сертификате.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct HTTP_SERVICE_CONFIG_SSL_KEY
        {
            public IntPtr pIpPort;

            public HTTP_SERVICE_CONFIG_SSL_KEY(IntPtr pIpPort)
            {
                this.pIpPort = pIpPort;
            }
        }

        /// <summary>
        ///     Значение записи о SSL-сертификате.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct HTTP_SERVICE_CONFIG_SSL_PARAM
        {
            public int SslHashLength;
            public IntPtr pSslHash;
            public Guid AppId;
            [MarshalAs(UnmanagedType.LPWStr)] public string pSslCertStoreName;
            public uint DefaultCertCheckMode;
            public int DefaultRevocationFreshnessTime;
            public int DefaultRevocationUrlRetrievalTimeout;
            [MarshalAs(UnmanagedType.LPWStr)] public string pDefaultSslCtlIdentifier;
            [MarshalAs(UnmanagedType.LPWStr)] public string pDefaultSslCtlStoreName;
            public uint DefaultFlags;
        }


        private const uint HTTP_INITIALIZE_CONFIG = 0x00000002;
        private const uint NOERROR = 0;
        private const uint ERROR_INSUFFICIENT_BUFFER = 122;
        private const uint ERROR_ALREADY_EXISTS = 183;
        private const uint ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_NO_MORE_ITEMS = 259;


        // ReSharper restore All
    }
}