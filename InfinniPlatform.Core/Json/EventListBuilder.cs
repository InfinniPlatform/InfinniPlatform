using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using InfinniPlatform.Api.Events;
using InfinniPlatform.Metadata;
using Newtonsoft.Json;

namespace InfinniPlatform.Json
{
    public static class EventBuilderExtensions
    {
        /// <summary>
        /// Возвращает <see cref="EventListBuilder"/> для объекта
        /// </summary>
        /// <param name="target">Объект, для которого необходимо составить список событий</param>
        /// <param name="rootName">Имя корневого узла</param>
        /// <returns></returns>
        public static EventListBuilder ToEventListBuilder(this object target, string rootName = "FormMetadata")
        {
            return new EventListBuilder(target, rootName);
        }

		public static EventListBuilder ToEventListBuilderDefault(this object target)
		{
			return new EventListBuilder(target, "");
		}

    }

    /// <summary>
    /// Инкапсулирует методы для формирования списка событий создания объекта
    /// </summary>
    public class EventListBuilder
    {
        public EventListBuilder(object root, string rootName)
        {
            _root = root;
            _rootName = rootName;
            _formMetadata = new ObjectMetadataHandler();
        }

        private readonly object _root;
        private readonly string _rootName;
        private readonly List<string> _propertyExclusion = new List<string>();
        private readonly ObjectMetadataHandler _formMetadata;

        /// <summary>
        /// Исключает узлы, перечисленные в <paramref name="propertyExclusions"/> из результирующей иерархии.
        /// <para>Попытка исключить корневой узел игнорируется.</para>
        /// </summary>
        /// <param name="propertyExclusions">Коллекция полных путей для узлов, которые необходимо исключить из обработки</param>
        public EventListBuilder Exclude(params string[] propertyExclusions)
        {
            _propertyExclusion.AddRange(propertyExclusions);
            return this;
        }

        /// <summary>
        /// Возвращает список строк, описывающих события генерации объекта. Для получения списка используется метод SyntaxNode.GetEvents()
        /// </summary>
        /// <returns>Коллекция строк, описывающих события генерации объекта</returns>
        public IEnumerable<EventDefinition> GetEventList()
        {
			return ParseObject(_root, _rootName).Select(e => (JsonConvert.DeserializeObject<EventDefinition>(e))).ToList();
        }

		public IEnumerable<string> GetEventListString()
		{
			return ParseObject(_root, _rootName).ToList();
		}

        private Dictionary<string, object> GetObjectProperties(object target)
        {
            return target
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(property => property.Name,
                              property => property.GetValue(target));
        }

        private bool IsValueType(object target)
        {
            return target == null || target is string || target.GetType().IsPrimitive;
        }

        private bool IsExcludedProperty(string path)
        {
            return _propertyExclusion.Any(x => new Regex(x).IsMatch(path));
        }

        private List<string> ParseObject(object target, string name, string path = null, bool isCollectionItem = false)
        {
            var fullName = String.IsNullOrEmpty(path) ? name : String.Join(".", path, name);
            if (IsExcludedProperty(fullName))
                return null;

            var result = new List<string>();
            if (IsValueType(target))
            {
                result.Add(isCollectionItem
                               ? _formMetadata.AddItemToCollection(fullName.Substring(0, fullName.LastIndexOf('.')), target)
                               : _formMetadata.CreateProperty(fullName, target));
            }
            else if (target is IEnumerable)
            {
                result.Add(_formMetadata.CreateContainerCollection(fullName));
                var index = 0;
                foreach (var item in (IEnumerable) target)
                {
                    var newEvents = ParseObject(item, (index++).ToString(), fullName, true);
                    if (newEvents != null)
                        result.AddRange(newEvents);
                }
            }
            else
            {
				if (!string.IsNullOrEmpty(fullName))
				{
					result.Add(isCollectionItem
								   ? _formMetadata.AddItemToCollection(fullName.Substring(0, fullName.LastIndexOf('.')))
								   : _formMetadata.CreateContainer(fullName));					
				}
                foreach (var property in GetObjectProperties(target))
                {
                    var newEvents = ParseObject(property.Value, property.Key, fullName);
                    if (newEvents != null)
                        result.AddRange(newEvents);
                }
            }

            return result;
        }
    }

}