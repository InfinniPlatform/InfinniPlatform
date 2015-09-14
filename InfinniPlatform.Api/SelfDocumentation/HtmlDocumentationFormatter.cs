using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace InfinniPlatform.Api.SelfDocumentation
{
    /// <summary>
    ///     Позволяет отформатировать информацию по REST запросам в виде HTML таблиц
    /// </summary>
    public sealed class HtmlDocumentationFormatter : IDocumentationFormatter
    {
        /// <summary>
        ///     Завершить форматирование целостного документа
        /// </summary>
        /// <param name="configuration">Имя конфигурации</param>
        /// <param name="content">Содержимое, которое необходимо оформить в виде документа</param>
        /// <returns>Отформатированный документ</returns>
        public string CompleteDocumentFormatting(string configuration, string content)
        {
            var stringWriter = new StringWriter();

            using (var writer = new HtmlTextWriter(stringWriter))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Html);

                writer.RenderBeginTag(HtmlTextWriterTag.Head);

                writer.AddAttribute(HtmlTextWriterAttribute.Content, "text/html; charset=utf-8");
                writer.RenderBeginTag(HtmlTextWriterTag.Meta);
                writer.RenderEndTag(); // Meta

                writer.RenderEndTag(); // Head


                writer.RenderBeginTag(HtmlTextWriterTag.Body);

                writer.RenderBeginTag(HtmlTextWriterTag.H2);
                writer.Write("Документация для конфигурации {0}", configuration);
                writer.RenderEndTag(); // H2

                writer.Write(content);
                writer.RenderEndTag(); // Body

                writer.RenderEndTag(); // Html
            }

            return stringWriter.ToString().Trim();
        }

        /// <summary>
        ///     Отформатировать серию запросов, объединенных общим заголовком
        /// </summary>
        /// <param name="header">Заголовок для блока справочной информации</param>
        /// <param name="info">Непосредственно запросы для форматирования</param>
        /// (указывается только если имя конфигурации одинаково для
        /// всех передаваемых запросов)
        /// <returns>Отформатированная справочная информация</returns>
        public string FormatQueries(string header, IEnumerable<RestQueryInfo> info)
        {
            var stringWriter = new StringWriter();

            using (var writer = new HtmlTextWriter(stringWriter))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.RenderBeginTag(HtmlTextWriterTag.H3);
                writer.Write(header);
                writer.RenderEndTag(); // H3

                var index = 1;

                foreach (var tableInfo in info)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.H4);
                    writer.WriteEncodedText(string.Format("Пример #{0}", index++));
                    writer.RenderEndTag(); // H4

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1");
                    writer.AddAttribute(HtmlTextWriterAttribute.Style, "table-layout: fixed;width: 100%;");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);

                    writer.RenderBeginTag(HtmlTextWriterTag.Colgroup);
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "8%");
                    writer.RenderBeginTag(HtmlTextWriterTag.Col);
                    writer.RenderEndTag(); // Col
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "92%");
                    writer.RenderBeginTag(HtmlTextWriterTag.Col);
                    writer.RenderEndTag(); // Col
                    writer.RenderEndTag(); // Colgroup

                    writer.RenderBeginTag(HtmlTextWriterTag.Thead);

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                    writer.RenderBeginTag(HtmlTextWriterTag.Th);
                    writer.WriteEncodedText("Параметр");
                    writer.RenderEndTag(); // Th

                    writer.RenderBeginTag(HtmlTextWriterTag.Th);
                    writer.WriteEncodedText("Значение");
                    writer.RenderEndTag(); // Th

                    writer.RenderEndTag(); // Tr
                    writer.RenderEndTag(); // Thead

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteEncodedText("Type");
                    writer.RenderEndTag(); // Td
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style,
                        "overflow: auto; white-space:pre; max-height: 500px;font-family: monospace;");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Indent = 0;
                    writer.WriteEncodedText(tableInfo.QueryType);
                    writer.RenderEndTag(); // Div
                    writer.RenderEndTag(); // Td
                    writer.RenderEndTag(); // Tr

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteEncodedText("Url");
                    writer.RenderEndTag(); // Td
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style,
                        "overflow: auto; white-space:pre; max-height: 500px;font-family: monospace;");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Indent = 0;
                    writer.WriteEncodedText(tableInfo.Url);
                    writer.RenderEndTag(); // Div
                    writer.RenderEndTag(); // Td
                    writer.RenderEndTag(); // Tr

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteEncodedText("Body");
                    writer.RenderEndTag(); // Td
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style,
                        "overflow: auto; white-space:pre; max-height: 500px;font-family: monospace;");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Indent = 0;
                    writer.WriteEncodedText(tableInfo.Body);
                    writer.RenderEndTag(); // Div
                    writer.RenderEndTag(); // Td
                    writer.RenderEndTag(); // Tr

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteEncodedText("Response");
                    writer.RenderEndTag(); // Td
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style,
                        "overflow: auto; white-space:pre; max-height: 500px;font-family: monospace;");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Indent = 0;
                    writer.WriteEncodedText(tableInfo.ResponceContent);
                    writer.RenderEndTag(); // Div
                    writer.RenderEndTag(); // Td
                    writer.RenderEndTag(); // Tr

                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteEncodedText("Example source");
                    writer.RenderEndTag(); // Td
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.AddAttribute(HtmlTextWriterAttribute.Style,
                        "overflow: auto; white-space:pre; max-height: 500px;font-family: monospace;");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Indent = 0;
                    writer.WriteEncodedText(tableInfo.ExampleSource);
                    writer.RenderEndTag(); // Div
                    writer.RenderEndTag(); // Td
                    writer.RenderEndTag(); // Tr

                    writer.RenderEndTag(); // Table
                }

                writer.RenderEndTag(); // Div
            }

            return stringWriter.ToString().Trim();
        }
    }
}