using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class ProjectConverterFrom : IProjectConverterFrom
    {
        public Project Convert(Stream projectText)
        {
            if (projectText == null)
                return null;

            var project = new Project();
            var projectXml = XDocument.Load(projectText);
            var root = projectXml.Root;

            if (root != null)
            {
                var ns = root.Attribute("xmlns").Value;

                project.ProjectProperty = GetProjectProperty(root, ns);
                project.Modules = GetItems<Module>(root, ns);
                project.References = GetItems<Reference>(root, ns);
                project.ProjectReferences = GetItems<ProjectReference>(root, ns);
                project.EmbeddedResources = GetItems<EmbeddedResource>(root, ns);
                project.Contents = GetItems<Content>(root, ns);
                project.Imports = GetItems<Import>(root, ns);
            }

            return project;
        }

        /// <summary>
        /// Сохраняет свойства проекта, такие как GUID, тип, имя и т.д.
        /// </summary>
        /// <param name="root">корневой элемент proj файла</param>
        /// <param name="ns">пространство имен данного xml</param>
        /// <returns>объект, хранящий важные характеристики проекта</returns>
        private ProjectProperty GetProjectProperty(XElement root, XNamespace ns)
        {
            var projectProperty = new ProjectProperty();

            var type = projectProperty.GetType();
            var elements = projectProperty.GetElements();

            if (elements != null)
            {
                foreach (ReflectionPair element in elements)
                {
                    var propertyInfo = type.GetProperty(element.PropertyName);
                    propertyInfo.SetValue(projectProperty, root.GetDescendantValue(element.XmlElementName, ns));
                }
            }

            return projectProperty;
        }

        /// <summary>
        /// Сохраняет все подключаемые ресурсы, соответсвующие типу T
        /// </summary>
        /// <typeparam name="T">класс, хранящий информацию о данном виде ресурса</typeparam>
        /// <param name="root">корневой элемент proj файла</param>
        /// <param name="ns">пространство имен данного xml</param>
        /// <returns>набор объектов, каждый из которых хранит информацию о используемом в проекте ресурсе</returns>
        private IEnumerable<T> GetItems<T>(XElement root, XNamespace ns) where T : IItemElement, new()
        {
            var items = new List<T>();
            var tagName = new T().XmlElementTagName;

            foreach (var itemTag in root.Descendants(ns + tagName))
            {
                var item = new T();
                SetElementValues(item, itemTag, ns);
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Сохраняет свойства элемент xml-документа, в объект соответствующего ему класса
        /// </summary>
        /// <param name="component"> Объект, в который сохранияется информация о элементе xml</param>
        /// <param name="componentTag">Элемент xml-документа, который требуется сохранить</param>
        /// <param name="ns">пространство имен данного xml</param>
        private void SetElementValues(IProjectComponent component, XElement componentTag, XNamespace ns)
        {
            var type = component.GetType();
            var attributes = component.GetAttributes();
            var elements = component.GetElements();

            if (attributes != null)
            {
                foreach (ReflectionPair attribute in attributes)
                {
                    var propertyInfo = type.GetProperty(attribute.PropertyName);
                    if (propertyInfo == null)
                        throw new Exception("Incorrect property name");
                    propertyInfo.SetValue(component, componentTag.GetAttributeValue(attribute.XmlElementName));
                }
            }

            if (elements != null)
            {
                foreach (ReflectionPair element in elements)
                {
                    var propertyInfo = type.GetProperty(element.PropertyName);
                    if (propertyInfo == null)
                        throw new Exception("Incorrect property name");
                    propertyInfo.SetValue(component, componentTag.GetElementValue(element.XmlElementName, ns));
                }
            }
        }
    }
}
