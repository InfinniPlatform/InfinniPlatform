using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InfinniPlatform.Core.SelfDocumentation
{
    /// <summary>
    ///     Хранит структурированную документацию по
    ///     REST запросам, позволяя делать выборки
    ///     (например, документацию по определенной конфигурации)
    /// </summary>
    public sealed class DocumentationKeeper
    {
        private readonly string _helpPath;
        private readonly IDocumentationFormatter _documentationFormatter;

        /// <summary>
        ///     Словарь содержит информацию по REST запросам, сгруппированную по комментарию
        /// </summary>
        private readonly HelpMultiDictionary _innerHelpContainer;

        public DocumentationKeeper(string helpPath, IDocumentationFormatter documentationFormatter)
        {
            _helpPath = helpPath;
            _documentationFormatter = documentationFormatter;
            _innerHelpContainer = new HelpMultiDictionary();
        }

        /// <summary>
        ///     Сохраняет документацию по конфигурации в заданном файле
        /// </summary>
        public void SaveHelp(string configuration)
        {
            var filePath = GetHelpFilePath(_helpPath, configuration);

            EnsureDirectory(Path.GetDirectoryName(filePath));

            File.WriteAllText(filePath, GetHelp(configuration));
        }

        public static string ReadHelpFromFile(string helpPath, string configuration)
        {
            var helpFilePath = GetHelpFilePath(helpPath, configuration);
            return File.Exists(helpFilePath) ? File.ReadAllText(helpFilePath) : String.Empty;
        }

        /// <summary>
        ///     Возвращает справочную информацию по всем конфигурациям
        /// </summary>
        public string GetHelp()
        {
            var helpString = new StringBuilder();

            foreach (var helpComment in _innerHelpContainer)
            {
                var helpInfo = _innerHelpContainer.GetHelpInfo(helpComment);

                if (helpInfo.Length > 0)
                {
                    helpString.AppendLine(_documentationFormatter.FormatQueries(helpComment, helpInfo));
                }
            }

            return _documentationFormatter.CompleteDocumentFormatting("Cправочная информация по всем конфигурациям",
                helpString.ToString());
        }

        /// <summary>
        ///     Возвращает справочную информацию для заданной конфигурации
        /// </summary>
        public string GetHelp(string configurationName)
        {
            var helpString = new StringBuilder();

            foreach (var helpComment in _innerHelpContainer)
            {
                var helpInfo = _innerHelpContainer.GetHelpInfo(helpComment, configurationName);

                if (helpInfo.Length > 0)
                {
                    helpString.AppendLine(_documentationFormatter.FormatQueries(helpComment, helpInfo));
                }
            }

            return _documentationFormatter.CompleteDocumentFormatting(configurationName, helpString.ToString());
        }

        public void AddHelpInfo(RestQueryInfo info)
        {
            _innerHelpContainer.Add(info.Comment, info);
        }

        private static void EnsureDirectory(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        private static string GetHelpFilePath(string helpPath, string configurationName)
        {
            var pathToHelp = !string.IsNullOrEmpty(helpPath) ? Path.GetFullPath(helpPath) : Directory.GetCurrentDirectory();
            var result = string.Format(Path.Combine(pathToHelp, @"{0}.html"), configurationName);
            return result;
        }

        /// <summary>
        ///     Внутренний словарь, в котором одному ключу соответствует несколько значений.
        ///     Облегчает операции поиска/выборки запросов для построения документации
        /// </summary>
        private class HelpMultiDictionary : IEnumerable<string>
        {
            private readonly Dictionary<string, List<RestQueryInfo>> _data =
                new Dictionary<string, List<RestQueryInfo>>();

            public IEnumerator<string> GetEnumerator()
            {
                return _data.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(string key, RestQueryInfo value)
            {
                if (_data.ContainsKey(key))
                {
                    _data[key].Add(value);
                }
                else
                {
                    _data.Add(key, new List<RestQueryInfo> {value});
                }
            }

            public RestQueryInfo[] GetHelpInfo(string key)
            {
                return _data.ContainsKey(key) ? _data[key].ToArray() : new RestQueryInfo[0];
            }

            public RestQueryInfo[] GetHelpInfo(string key, string configurationName)
            {
                return _data.ContainsKey(key)
                    ? _data[key].Where(
                        d =>
                            String.Equals(d.Configuration, configurationName,
                                StringComparison.InvariantCultureIgnoreCase)).ToArray()
                    : new RestQueryInfo[0];
            }
        }
    }
}