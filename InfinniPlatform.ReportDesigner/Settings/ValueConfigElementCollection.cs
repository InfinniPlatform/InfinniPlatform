using System;
using System.Configuration;

namespace InfinniPlatform.ReportDesigner.Settings
{
	public class ValueConfigElementCollection : ConfigurationElementCollection
	{
		private static readonly ConfigurationPropertyCollection PropertiesCollection;


		static ValueConfigElementCollection()
		{
			PropertiesCollection = new ConfigurationPropertyCollection();
		}


		protected override string ElementName
		{
			get { return "item"; }
		}

		protected override ConfigurationPropertyCollection Properties
		{
			get { return PropertiesCollection; }
		}

		public override ConfigurationElementCollectionType CollectionType
		{
			get { return ConfigurationElementCollectionType.BasicMap; }
		}


		protected override ConfigurationElement CreateNewElement()
		{
			return new ValueConfigElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			var valueElement = element as ValueConfigElement;

			if (valueElement == null)
			{
				throw new ArgumentNullException();
			}

			return valueElement.Value;
		}
	}
}