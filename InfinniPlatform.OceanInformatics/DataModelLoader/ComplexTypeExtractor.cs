using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.ModelRepository;
using InfinniPlatform.Core.ModelRepository.MetadataObjectModel;

namespace InfinniPlatform.OceanInformatics.DataModelLoader
{
    public class ComplexTypeExtractor : IComplexTypeExtractor
    {
        private readonly string _configId;

        private readonly List<string> _typeNames;
        
        public ComplexTypeExtractor(string configId)
        {
            _configId = configId;
            _typeNames = new List<string>();
        }

        /// <summary>
        /// Извлечение метаданных сложных типов производится на основе отражения. Дело в том, что
        /// классы со сложными типами OpenEHR определены в сборке OpenEhr.V1.Its.Xml.dll. Для загрузки типов
        /// можно конечно использовать xsd схему BaseTypes, но этот файл сгенерирован на основе данных из сборки.
        /// Поэтому первичным источником является сборка, а не схема. Помимо этого, разбор сборки сделать проще,
        /// чем анализ xsd схемы
        /// </summary>
        public IDictionary<string, DataSchema> ExtractComplexTypeModels()
        {
            var complexTypes = new Dictionary<string, DataSchema>();

            string assemblyPath = Path.GetFullPath("OpenEhr.V1.Its.Xml.dll");

            // Creation AssemblyName which help load an assembly
            var assemblyName = new AssemblyName
            {
                CodeBase = assemblyPath
            };

            // try to load assembly
            var assembly = Assembly.Load(assemblyName);

            // select all classes whose name starts from "DV_" and marked with System.Xml.Serialization.XmlTypeAttribute
            var dvTypes = (from t in assembly.GetTypes()
                              where (t.IsClass &&
                              Attribute.IsDefined(t, typeof(XmlTypeAttribute)) &&
                              t.Name.StartsWith("DV_"))
                              select t).ToArray();

            _typeNames.AddRange(dvTypes.Select(t => t.Name));

            foreach (var dvType in dvTypes)
            {
                var complexType = new DataSchema
                {
                    Caption = dvType.Name,
                    Type = DataType.Object.ToString()
                };

                foreach (var property in dvType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var propertyModel = ExtractPropertyModel(property);
                    complexType.Properties.Add(propertyModel.Value != null
                        ? propertyModel
                        : ExtractSubtypeModel(property));
                }
                
                complexTypes[dvType.Name] = complexType;
            }

            return complexTypes;
        }

        private KeyValuePair<string, DataSchema> ExtractSubtypeModel(PropertyInfo property)
        {
            if (property.PropertyType.IsArray)
            {
                var complexType = new DataSchema
                {
                    Caption = property.Name,
                    Type = DataType.Array.ToString()
                };

                var arrayItem = new DataSchema
                {
                    Type = DataType.Object.ToString()
                };

                foreach (var childProperty in 
                    property.PropertyType.GetElementType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var propertyModel = ExtractPropertyModel(childProperty);
                    arrayItem.Properties.Add(propertyModel.Value != null
                        ? propertyModel
                        : ExtractSubtypeModel(childProperty));
                }

                complexType.Items = arrayItem;

                return new KeyValuePair<string, DataSchema>(property.Name, complexType);
            }
            else
            {
                var complexType = new DataSchema
                {
                    Caption = property.Name,
                    Type = DataType.Object.ToString()
                };

                foreach (var childProperty in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var propertyModel = ExtractPropertyModel(childProperty);
                    complexType.Properties.Add(propertyModel.Value != null
                        ? propertyModel
                        : ExtractSubtypeModel(childProperty));
                }

                return new KeyValuePair<string, DataSchema>(property.Name, complexType);
            }
        }

        private KeyValuePair<string, DataSchema> ExtractPropertyModel(PropertyInfo property)
        {
            var complexType = new DataSchema();

            if (_typeNames.Contains(property.PropertyType.Name))
            {
                var model = new DataSchema
                {
                    Caption = property.Name,
                    Type = DataType.Object.ToString(),
                };

                model.TypeInfo.Add("DocumentLink", new DocumentLink
                {
                    ConfigId = _configId,
                    DocumentId = property.PropertyType.Name,
                    Inline = true
                });

                return new KeyValuePair<string, DataSchema>(property.Name, model);
            }

            switch (property.PropertyType.Name)
            {
                case "String":
                    complexType.Type = DataType.String.ToString();
                    break;
                case "Boolean":
                    complexType.Type = DataType.Bool.ToString();
                    break;
                case "DateTime":
                    complexType.Type = DataType.DateTime.ToString();
                    break;
                case "Double":
                case "float":
                    complexType.Type = DataType.Float.ToString();
                    break;
                case "Int16":
                case "Int32":
                case "Int64":
                    complexType.Type = DataType.Integer.ToString();
                    break;
                case "Byte[]":
                    complexType.Type = DataType.Binary.ToString();
                    break;
                default:
                    // it is a complex type...
                    return new KeyValuePair<string, DataSchema>("null", null);
            }

            return new KeyValuePair<string, DataSchema>(property.Name, complexType);
        }
    }
}
