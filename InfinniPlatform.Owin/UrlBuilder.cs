using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Owin.Helpers;

namespace InfinniPlatform.Owin
{
    /// <summary>
    /// Предоставляет методы для формирования URI.
    /// </summary>
    public sealed class UrlBuilder
    {
        private static readonly Uri BaseUri = new Uri("http://localhost");

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="address">Адресная строка.</param>
        public UrlBuilder(string address = null)
        {
            if (!string.IsNullOrWhiteSpace(address))
            {
                var addressUri = new Uri(address, UriKind.RelativeOrAbsolute);

                string baseAddress = null;
                string queryText = null;

                if (addressUri.IsAbsoluteUri)
                {
                    baseAddress = $"{addressUri.Scheme}://{addressUri.Host}:{addressUri.Port}{addressUri.LocalPath}";
                    queryText = addressUri.Query;
                }
                else if (Uri.TryCreate(BaseUri, address, out addressUri))
                {
                    baseAddress = addressUri.LocalPath;
                    queryText = addressUri.Query;
                }

                _baseAddress = baseAddress;

                if (!string.IsNullOrEmpty(queryText))
                {
                    foreach (var item in WebHelpers.ParseForm(queryText.TrimStart('?')))
                    {
                        if (item.Value != null && item.Value.Length > 0)
                        {
                            _query[item.Key] = item.Value[0];
                        }
                    }
                }
            }
        }


        private string _baseAddress;
        private readonly Dictionary<string, object> _query = new Dictionary<string, object>();


        /// <summary>
        /// Устанавливает базовый адрес для относительного URI.
        /// </summary>
        public UrlBuilder Relative(string baseAddress)
        {
            _baseAddress = baseAddress;

            return this;
        }

        /// <summary>
        /// Устанавливает базовый адрес для абсолютного URI.
        /// </summary>
        public UrlBuilder Absolute(string serverScheme, string serverName, int? serverPort = null)
        {
            if (string.IsNullOrWhiteSpace(serverScheme))
            {
                throw new ArgumentNullException(nameof(serverScheme));
            }

            if (string.IsNullOrWhiteSpace(serverName))
            {
                throw new ArgumentNullException(nameof(serverName));
            }

            if (serverPort <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(serverPort));
            }

            _baseAddress = (serverPort != null)
                ? $"{serverScheme}://{serverName}:{serverPort}"
                : $"{serverScheme}://{serverName}";

            return this;
        }

        /// <summary>
        /// Добавляет параметры запроса URI.
        /// </summary>
        public UrlBuilder AddQuery(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _query[name] = value;

            return this;
        }

        /// <summary>
        /// Возвращает строковое представление URI.
        /// </summary>
        public override string ToString()
        {
            var result = new StringBuilder(_baseAddress);

            if (_query.Count > 0)
            {
                var separator = '?';

                foreach (var q in _query)
                {
                    var name = Uri.EscapeDataString(q.Key);
                    var value = (q.Value != null) ? Uri.EscapeDataString(q.Value.ToString()) : string.Empty;

                    result.Append(separator);
                    result.Append(name);
                    result.Append('=');
                    result.Append(value);

                    separator = '&';
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Возвращает объектное представление URI.
        /// </summary>
        public Uri ToUri()
        {
            return new Uri(ToString());
        }
    }
}