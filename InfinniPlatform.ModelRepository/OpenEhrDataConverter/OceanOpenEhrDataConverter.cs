using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using InfinniPlatform.Core.ModelRepository.DataConverters;
using InfinniPlatform.Core.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.ModelRepository.OpenEhrDataConverter
{
    public sealed class OceanOpenEhrDataConverter : IDataConverter
    {
        /// <summary>
        /// Преобразует блок данных в JSON документ определенной структуры 
        /// </summary>
        /// <param name="sourceData">Исходные данные для конвертации</param>
        /// <param name="documentModel">Структура получаемого документа</param>
        /// <param name="inlineDocuments">Набор схем встроенных документов</param>
        /// <param name="errorMessage">Ошибки, возникшие в ходе преобразования</param>
        /// <param name="extractedData">Преобразованный документ</param>
        /// <returns>Результат конвертации, если true - конвертация
        /// завершилась успешно, false - нет (ошибки содержатся в поле errorMessage)</returns>
        public bool ConvertData(
            Stream sourceData,
            DataSchema documentModel,
            IDictionary<string, DataSchema> inlineDocuments,
            out string errorMessage,
            out JObject extractedData)
        {
            var errorKeeper = new StringBuilder();
            extractedData = new JObject();
            
            try
            {
                var souceDataContent = XDocument.Load(sourceData).Root;

                if (souceDataContent != null)
                {
                    var defaultNamespace = souceDataContent.GetDefaultNamespace().NamespaceName;

                    if (string.IsNullOrEmpty(defaultNamespace))
                    {
                        // Это временное решение (для поддержки документов ЧХМАО), 
                        // вообще в документе должно быть задано пространство имен
                        defaultNamespace = "http://schemas.oceanehr.com/templates";
                    }
                    
                    var xsiNamespace = souceDataContent.GetNamespaceOfPrefix("xsi");
                    if (xsiNamespace == null)
                    {
                        errorKeeper.AppendLine();
                        errorKeeper.AppendFormat(
                            "Expected namespace 'xsi' is not found. Without namespace it is impossible to get types of inner items");
                    }
                    else
                    {
                        var propertyGroupExtractor = new ObjectDataExtractor(
                            errorKeeper,
                            inlineDocuments,
                            defaultNamespace,
                            xsiNamespace.NamespaceName);

                        extractedData = propertyGroupExtractor.ExtractData(souceDataContent, documentModel.Properties.Values.First());
                    }
                }
            }
            catch (Exception e)
            {
                errorKeeper.AppendLine();
                errorKeeper.AppendFormat("Document parsing error: {0}", e.Message);
            }

            errorMessage = errorKeeper.ToString();

            return string.IsNullOrEmpty(errorMessage);
            
        }
    }
}
