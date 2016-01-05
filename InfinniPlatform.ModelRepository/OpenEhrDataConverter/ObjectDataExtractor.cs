using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using InfinniPlatform.Core.Extensions;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.ModelRepository.OpenEhrDataConverter
{
    internal sealed class ObjectDataExtractor
    {
        private readonly StringBuilder _errorKeeper;
        private readonly string _defaultNamespace;
        private readonly PropertyDataExtractor _propertyExtractor;
        
        internal ObjectDataExtractor(
            StringBuilder errorKeeper,
            IDictionary<string, DataSchema> inlineDocuments, 
            string defaultNamespace, 
            string xsiNamespace)
        {
            _errorKeeper = errorKeeper;
            _defaultNamespace = defaultNamespace;
            _propertyExtractor = new PropertyDataExtractor(errorKeeper, inlineDocuments, defaultNamespace, xsiNamespace);
        }

        internal JObject ExtractData(XElement content, DataSchema groupModel)
        {
            var groupItems = new JObject();
            string errorMessage;
            string nodeId;

            var dataTag = content.Element(XName.Get("data", _defaultNamespace));
            if (dataTag != null)
            {
                // если элемент имеет вложенный тег 'data', то проходить по всем
                // дочерним элементам нет смысла. Просто конвертируем содержимое тега 'data' в группу
                // (сделано из соображений быстродействия)
                if (dataTag.ExtractAttributeValue(XName.Get("archetype_node_id"), out nodeId, out errorMessage))
                {
                    var childGroupModel = groupModel.Properties.SingleOrDefault(g => g.Value.TypeInfo["NodeId"].ToString() == nodeId);
                    if (childGroupModel.Value == null)
                    {
                        _errorKeeper.AppendLine();
                        _errorKeeper.AppendFormat("Expected group {0} is not found in the group model {1}", nodeId, groupModel.Caption);
                    }
                    else
                    {
                        groupItems.Add(childGroupModel.Key, ExtractData(dataTag, childGroupModel.Value));
                    }
                }
                else
                {
                    _errorKeeper.Append(errorMessage);
                }
            }
            else
            {
                foreach (var item in content.Elements())
                {
                    if (!item.ExtractAttributeValue(XName.Get("archetype_node_id"), out nodeId, out errorMessage))
                    {
                        // skip elements without node id
                        continue;
                    }

                    var childModel = 
                        groupModel.Type == DataType.Object.ToString() ? 
                        groupModel.Properties.Where(g => g.Value.TypeInfo["NodeId"].ToString() == nodeId).ToList() : 
                        groupModel.Items.Properties.Where(g => g.Value.TypeInfo["NodeId"].ToString() == nodeId).ToList();
                        

                    string nodeName;
                    if (!item.ExtractElementValueByXPath(out nodeName, out errorMessage, XName.Get("name", _defaultNamespace)))
                    {
                        _errorKeeper.AppendLine();
                        _errorKeeper.AppendFormat("Group {0} parsing error: {1}", groupModel.Caption, errorMessage);
                        continue;
                    }

                    nodeName = RemoveDollarsFromNameAndTransliterate(nodeName);

                    JObject extractedObject;
                    bool isArrayItem;

                    if (childModel.Count == 1 && 
                        childModel.First().Value.Properties != null &&
                        childModel.First().Value.Properties.Count == 0)
                    {
                        // may be it is a property, not a group
                        var childPropertiesModel = 
                            groupModel.Type == DataType.Object.ToString() ? 
                            groupModel.Properties.Where(g => g.Value.TypeInfo["NodeId"].ToString() == nodeId).ToList() :
                            groupModel.Items.Properties.Where(g => g.Value.TypeInfo["NodeId"].ToString() == nodeId).ToList();

                        if (childPropertiesModel.Count == 0)
                        {
                            _errorKeeper.AppendLine();
                            _errorKeeper.AppendFormat("Expected item {0} is not found in the group model {1}", nodeId, groupModel.Caption);
                            continue;
                        }

                        // find required property model
                        var childPropertyModel =
                            childPropertiesModel.Count > 1 ?
                            childPropertiesModel.FirstOrDefault(m => m.Key == nodeName) : childPropertiesModel.First();

                        if (childPropertyModel.Value == null)
                        {
                            _errorKeeper.AppendLine();
                            _errorKeeper.AppendFormat("Expected item with name {0} is not found in the group model {1}", nodeName, groupModel.Caption);
                            continue;
                        }

                        extractedObject = _propertyExtractor.ExtractData(item, childPropertyModel.Value);
                        isArrayItem = childPropertyModel.Value.Type == DataType.Array.ToString();
                    }
                    else
                    {
                        // find required object model
                        var childGroupModel =
                            childModel.Count > 1 ?
                            childModel.FirstOrDefault(m => m.Key == nodeName) : childModel.First();

                        if (childGroupModel.Value == null)
                        {
                            _errorKeeper.AppendLine();
                            _errorKeeper.AppendFormat("Expected item with name {0} is not found in the group model {1}", nodeName, groupModel.Caption);
                            continue;
                        }

                        extractedObject = ExtractData(item, childGroupModel.Value);
                        isArrayItem = childGroupModel.Value.Type == DataType.Array.ToString();
                    }

                    if (!isArrayItem)
                    {
                        groupItems.Add(nodeName, extractedObject);
                    }
                    else
                    {
                        // it is possible situation when several items in one group have the same name,
                        // so we need to convert them to an array
                        if (groupItems[nodeName] == null)
                        {
                            groupItems.Add(nodeName, new JArray { extractedObject });
                        }
                        else
                        {
                            // add item to existing array
                            ((JArray)groupItems[nodeName]).Add(extractedObject);
                        }
                    }
                }
            }

            return groupItems;
        }

        private static string RemoveDollarsFromNameAndTransliterate(string nodeName)
        {
            return nodeName.Split(new[] {"$", "\n"}, StringSplitOptions.RemoveEmptyEntries)[0].TrimStart(' ').ToTranslit();
        }
    }
}
