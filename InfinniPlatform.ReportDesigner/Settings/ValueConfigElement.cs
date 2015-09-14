using System.Configuration;

namespace InfinniPlatform.ReportDesigner.Settings
{
    public class ValueConfigElement : ConfigurationElement
    {
        private static readonly ConfigurationProperty KeyProperty;
        private static readonly ConfigurationProperty ValueProperty;
        private static readonly ConfigurationPropertyCollection PropertyCollection;

        static ValueConfigElement()
        {
            KeyProperty = new ConfigurationProperty("key", typeof (string), null, ConfigurationPropertyOptions.IsKey);
            ValueProperty = new ConfigurationProperty("value", typeof (string), null,
                ConfigurationPropertyOptions.IsRequired);
            PropertyCollection = new ConfigurationPropertyCollection {KeyProperty, ValueProperty};
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return PropertyCollection; }
        }

        public string Key
        {
            get { return this[KeyProperty] as string; }
            set { this[KeyProperty] = value; }
        }

        public string Value
        {
            get { return this[ValueProperty] as string; }
            set { this[ValueProperty] = value; }
        }
    }
}