using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class ProjectConverterTo : IProjectConverterTo
    {
        private readonly ProjectConfig _config;

        public ProjectConverterTo(ProjectConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Создает на основе информации из объекта класса Project файл .csproj
        /// </summary>
        /// <param name="initialProject">Объект класса Project, содержащий информацию о .csproj файле</param>
        /// <returns>Поток, содержащий полученный .csproj файл</returns>
        public Stream Convert(Project initialProject)
        {
            XNamespace ns = @"http://schemas.microsoft.com/developer/msbuild/2003";

            if (initialProject == null)
                return null;

            var resultProject = CreateBaseProj(initialProject, ns);
            var root = resultProject.Root;

            if (root != null)
            {
                root.Add(CreateItemsGroup(initialProject.References, ns));
                root.Add(CreateItemsGroup(initialProject.Modules, ns));
                root.Add(CreateItemsGroup(initialProject.ProjectReferences, ns));
                root.Add(CreateItemsGroup(initialProject.EmbeddedResources, ns));
                root.Add(CreateItemsGroup(initialProject.Contents, ns));
                root.Add(CreateImportsList(initialProject, ns));
            }

            Stream result = new MemoryStream();
            using (var writer = XmlWriter.Create(result))
            {
                resultProject.WriteTo(writer);
                writer.Flush();
            }
            return result;
        }

        /// <summary>
        /// На основе информации из _config и initialProject создается каскад .proj файла
        /// </summary>
        private XDocument CreateBaseProj(Project initialProject, XNamespace ns)
        {
            var doc = new XDocument();
            var root = new XElement(ns + "Project");
            doc.Add(root);

            var rootAttributes = new Hashtable
                {
                    {"ToolsVersion", "4.0"},
                    {"DefaultTargets", "Build"},
                    {"xmlns", ns}
                };
            root.Add(CreateAttributesArrayFromHashtable(rootAttributes));

            root.Add(CreateProjectProperty(initialProject, ns));

            var propertyGroupDebug = new XElement(ns + "PropertyGroup");
            propertyGroupDebug.SetAttributeValue("Condition", @" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ");
            var propertyGroupDebugElements = CreateElements(new Hashtable
                {
                    {"DebugType", "full"},
                    {"Optimize", "false"},
                    {"OutputPath", _config.DebugOutputPath},
                    {"DefineConstants", "DEBUG;TRACE"}
                },
                ns);
            propertyGroupDebug.Add(propertyGroupDebugElements);
            root.Add(propertyGroupDebug);

            var propertyGroupRelease = new XElement(ns + "PropertyGroup");
            propertyGroupRelease.SetAttributeValue("Condition", @" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ");
            var propertyGroupReleaseElements = CreateElements(new Hashtable
                {
                    {"DebugType", "pdbonly"},
                    {"Optimize", "true"},
                    {"OutputPath", _config.ReleaseOutputPath},
                    {"DefineConstants", "TRACE"}
                },
                ns);
            propertyGroupRelease.Add(propertyGroupReleaseElements);
            root.Add(propertyGroupRelease);

            return doc;
        }

        /// <summary>
        /// Задает свойства проекта(GUID, тип, имя и т.д.)
        /// </summary>
        /// <param name="initialProject">объект, хранящий основные свойства проекта</param>
        /// <param name="ns">пространство имен данного xml</param>
        /// <returns>объект XElement, содержащий PropertyGroup</returns>
        private XElement CreateProjectProperty(Project initialProject, XNamespace ns)
        {
            if (initialProject.ProjectProperty == null)
                return null;

            var propertyGroup = CreateElement("PropertyGroup", initialProject.ProjectProperty, ns);

            var properties = new[]
                {
                 new XElement(ns + "TargetFrameworkVersion", _config.TargetFrameworkVersion)
                };
            propertyGroup.Add(properties);

            return propertyGroup;
        }

        /// <summary>
        /// Задает подключаемые ресурсы проекта
        /// </summary>
        /// <param name="items">Подключаемые в проект ресурсы</param>
        /// <param name="ns">пространство имен данного xml</param>
        /// <returns>объект XElement, содержащий ItemGroup с указанными ресурсами</returns>
        private XElement CreateItemsGroup(IEnumerable<IItemElement> items, XNamespace ns)
        {
            if (items == null) return null;
            var itemList = items as IList<IItemElement> ?? items.ToList();
            if (!IsHaveAnyElements(itemList))
                return null;

            var itemsGroup = new XElement(ns + "ItemGroup");

            foreach (var item in itemList)
            {
                itemsGroup.Add(CreateElement(item.XmlElementTagName, item, ns));
            }

            return itemsGroup;
        }

        private IEnumerable<XElement> CreateImportsList(Project initialProject, XNamespace ns)
        {
            if (!IsHaveAnyElements(initialProject.Imports))
                return null;

            return initialProject.Imports
                                    .Select(import => CreateElement("Import", import, ns))
                                    .ToList();
        }

        private bool IsHaveAnyElements(IEnumerable<Object> enumerable)
        {
            return (enumerable != null && enumerable.Any());
        }

        private XAttribute[] CreateAttributesArrayFromHashtable(Hashtable attributes)
        {
            var attributesList = from DictionaryEntry attribute in attributes
                                 select new XAttribute(attribute.Key.ToString(), attribute.Value.ToString());
            return attributesList.ToArray();
        }

        /// <summary>
        /// Создает объект XElement на основе данных из IProjectComponent
        /// </summary>
        /// <param name="name"> Тэг, характеризующий данный объект</param>
        /// <param name="data"> Информация о создаваемом объекте</param>
        /// <param name="ns">пространство имен данного xml</param>
        /// <returns> Элемент XElement, соответствующий содержимомму IProjectComponent</returns>
        private XElement CreateElement(string name, IProjectComponent data, XNamespace ns)
        {
            var result = new XElement(ns + name);
            var type = data.GetType();
            var attributes = data.GetAttributes();
            var tags = data.GetElements();

            if (attributes != null)
            {
                foreach (ReflectionPair attribute in attributes)
                {
                    var attributeName = attribute.XmlElementName;
                    var propertyInfo = type.GetProperty(attribute.PropertyName);
                    if (propertyInfo == null)
                        throw new Exception("Incorrect property name");
                    var attributeValue = propertyInfo.GetValue(data);
                    if (attributeValue != null)
                    {
                        result.SetAttributeValue(attributeName, attributeValue);
                    }
                }
            }

            if (tags != null)
            {
                foreach (ReflectionPair tag in tags)
                {
                    var elementName = tag.XmlElementName;
                    var propertyInfo = type.GetProperty(tag.PropertyName);
                    if (propertyInfo == null)
                        throw new Exception("Incorrect property name");
                    var elementValue = propertyInfo.GetValue(data);
                    if (elementValue != null)
                    {
                        result.Add(new XElement(ns + elementName, elementValue.ToString()));
                    }
                }
            }

            return result;
        }

        public XElement[] CreateElements(Hashtable elements, XNamespace ns)
        {
            var elementsList =
                elements.Cast<DictionaryEntry>()
                        .Select(
                            element => new XElement(ns + element.Key.ToString(), element.Value.ToString()));

            return elementsList.ToArray();
        }
    }
}
