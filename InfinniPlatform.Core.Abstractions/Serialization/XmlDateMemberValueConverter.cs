using System;
using System.Reflection;
using System.Xml.Serialization;

using InfinniPlatform.Types;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Определяет правила преобразования для свойств типа <see cref="DateTime" />,
    /// помеченных атрибутом <see cref="XmlElementAttribute" /> с типом данных
    /// <see cref="XmlElementAttribute.DataType" /> равным "date".
    /// </summary>
    public class XmlDateMemberValueConverter : IMemberValueConverter
    {
        /// <inheritdoc />
        public bool CanConvert(MemberInfo member)
        {
            var property = member as PropertyInfo;

            if (property != null && (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?)))
            {
                var xmlElementAttribute = property.GetCustomAttribute<XmlElementAttribute>();

                return (xmlElementAttribute != null && string.Equals(xmlElementAttribute.DataType, "date", StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        /// <inheritdoc />
        public object Convert(object value)
        {
            var date = value as DateTime?;

            if (date != null)
            {
                return (Date)date.Value;
            }

            return null;
        }

        /// <inheritdoc />
        public object ConvertBack(Func<Type, object> value)
        {
            var date = (Date?)value(typeof(Date?));

            if (date != null)
            {
                return (DateTime)date.Value;
            }

            return null;
        }
    }
}