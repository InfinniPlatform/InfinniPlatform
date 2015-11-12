using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
    internal sealed class MetadataReaderDocumentElement : MetadataReader
    {
        public MetadataReaderDocumentElement(string configurationId, string documentId, IMetadataContainerInfo metadataContainerInfo)
            : base(configurationId, metadataContainerInfo.GetMetadataTypeName())
        {
            _documentId = documentId;
            _metadataContainerInfo = metadataContainerInfo;
        }

        private readonly string _documentId;
        private readonly IMetadataContainerInfo _metadataContainerInfo;

        public override IEnumerable<dynamic> GetItems()
        {
            dynamic result =
                QueryMetadata.QueryConfiguration(QueryMetadata.GetDocumentMetadataShortListIql(ConfigurationId, _documentId,
                    _metadataContainerInfo.GetMetadataContainerName())).FirstOrDefault();

            if (result != null)
            {
                var filteredResult = result[_metadataContainerInfo.GetMetadataContainerName()];

                if (filteredResult != null)
                {
                    return DynamicWrapperExtensions.ToEnumerable(filteredResult);
                }

                return new List<dynamic>();
            }
            return new List<dynamic>();
        }

        public override dynamic GetItem(string metadataName)
        {
            var result =
                QueryMetadata.QueryConfiguration(QueryMetadata.GetDocumentMetadataByNameIql(ConfigurationId, _documentId, metadataName,
                    _metadataContainerInfo.GetMetadataContainerName(), _metadataContainerInfo.GetMetadataTypeName()))
                             .FirstOrDefault();

            //если наши метаданные элемента документа
            if (result != null)
            {
                dynamic metadataList = result[_metadataContainerInfo.GetMetadataContainerName()];

                //то возвращаем найденные метаданные элемента
                dynamic resultItem = result != null && metadataList != null && metadataList.Count > 0
                    ? metadataList[0][string.Format("{0}Full", _metadataContainerInfo.GetMetadataTypeName())]
                    : null;


                if (resultItem != null)
                {
                    ConvertStringToJsonProperties(resultItem);
                }
                return resultItem;
            }

            return null;
        }

        /// <summary>
        /// Преобразовать свойства JSON, хранимые в виде строк в JSON объекты
        /// </summary>
        /// <param name="resultItem">JSON объект</param>
        private void ConvertStringToJsonProperties(dynamic resultItem)
        {
            if (_metadataContainerInfo.GetMetadataTypeName() == MetadataType.View && resultItem != null)
            {
                dynamic layoutPanel = resultItem.LayoutPanel;
                if (layoutPanel != null && !(layoutPanel is string))
                {
                    try
                    {
                        dynamic jsonLayoutPanel = ((string)layoutPanel.JsonString).ToDynamic();
                        resultItem.LayoutPanel = jsonLayoutPanel;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse view layout panel: {0}", e.Message));
                    }
                }
                dynamic childViews = resultItem.ChildViews;
                if (childViews != null && !(childViews is string))
                {
                    try
                    {
                        dynamic jsonChildViews = ((string)childViews.JsonString).ToDynamicList();
                        resultItem.ChildViews = jsonChildViews;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse child views layout: {0}", e.Message));
                    }
                }
                dynamic dataSources = resultItem.DataSources;
                if (dataSources != null && !(dataSources is JArray) && dataSources.JsonString != null)
                {
                    try
                    {
                        dynamic jsonDataSources = ((string)dataSources.JsonString).ToDynamicList();
                        resultItem.DataSources = jsonDataSources;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse view datasources: {0}", e.Message));
                    }
                }
                dynamic parameters = resultItem.Parameters;
                if (parameters != null && !(parameters is JArray) && parameters.JsonString != null)
                {
                    try
                    {
                        dynamic jsonParameters = ((string)parameters.JsonString).ToDynamicList();
                        resultItem.Parameters = jsonParameters;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse view parameters: {0}", e.Message));
                    }
                }
            }
            if ((_metadataContainerInfo.GetMetadataTypeName() == MetadataType.ValidationError ||
                 _metadataContainerInfo.GetMetadataTypeName() == MetadataType.ValidationWarning) && resultItem != null)
            {
                dynamic validationOperator = resultItem.ValidationOperator;
                if (validationOperator != null && validationOperator.StringifiedJson != null &&
                    validationOperator.StringifiedJson == true)
                {
                    try
                    {
                        dynamic jsonValidationOperator =
                            ((string)validationOperator.JsonString).ToDynamic();
                        resultItem.ValidationOperator = jsonValidationOperator;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse validation operator: {0}", e.Message));
                    }
                }
            }
            if (_metadataContainerInfo.GetMetadataTypeName() == MetadataType.PrintView && resultItem != null)
            {
                dynamic printView = resultItem.Blocks;
                if (printView != null && printView.StringifiedJson != null && printView.StringifiedJson == true)
                {
                    try
                    {
                        dynamic jsonPrintView = ((string)printView.JsonString).ToDynamicList();
                        resultItem.Blocks = jsonPrintView;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse print view: {0}", e.Message));
                    }
                }
            }
        }
    }
}