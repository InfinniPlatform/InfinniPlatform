using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace InfinniPlatform.Reporting.DataProviders
{
	/// <summary>
	/// Провайдер для доступа к данным в формате XML.
	/// </summary>
	sealed class XmlDataProvider : IDataProvider
	{
		public XmlDataProvider(XElement rootElement)
		{
			// Корневой элемент должен содержать коллекцию объектов

			_elements = (rootElement != null) ? new XElementWrapper(rootElement) : Enumerable.Empty<XElementWrapper>();
		}


		private readonly IEnumerable<XElementWrapper> _elements;


		public object GetPropertyValue(object instance, string propertyName)
		{
			object propertyValue;
			TryGetValue(instance, propertyName, out propertyValue);

			return propertyValue;
		}

		public static bool TryGetValue(object instance, string propertyName, out object propertyValue)
		{
			var success = false;

			propertyValue = null;

			var xElement = instance as XElementWrapper;

			if (xElement != null)
			{
				propertyValue = xElement.Property(propertyName);
				success = true;
			}

			return success;
		}


		public IEnumerator GetEnumerator()
		{
			return _elements.GetEnumerator();
		}


		sealed class XElementWrapper : IEnumerable<XElementWrapper>
		{
			public XElementWrapper(XElement element)
			{
				_element = element;
			}


			private readonly XElement _element;


			public object Property(string propertyName)
			{
				object result = null;

				var valueAttribute = FirstAttribute(propertyName);

				if (valueAttribute != null)
				{
					result = valueAttribute.Value;
				}
				else
				{
					var valueElement = FirstElement(propertyName);

					if (valueElement != null)
					{
						if (valueElement.HasAttributes || valueElement.HasElements)
						{
							result = new XElementWrapper(valueElement);
						}
						else
						{
							var value = valueElement.Value;

							if (string.IsNullOrEmpty(value) == false)
							{
								result = value;
							}
						}
					}
				}

				return result;
			}

			private XAttribute FirstAttribute(string name)
			{
				return _element.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, name, StringComparison.OrdinalIgnoreCase));
			}

			private XElement FirstElement(string name)
			{
				return _element.Elements().FirstOrDefault(e => string.Equals(e.Name.LocalName, name, StringComparison.OrdinalIgnoreCase));
			}


			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public IEnumerator<XElementWrapper> GetEnumerator()
			{
				return _element.Elements().Select(e => new XElementWrapper(e)).GetEnumerator();
			}
		}
	}
}