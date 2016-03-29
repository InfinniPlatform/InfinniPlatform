using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// Реестр <see cref="IMemberValueConverter" />.
    /// </summary>
    public static class MemberValueConverterRegistry
    {
        private static readonly List<IMemberValueConverter> ConverterList = new List<IMemberValueConverter>();


        /// <summary>
        /// Список зарегистрированных <see cref="IMemberValueConverter"/>.
        /// </summary>
        public static IEnumerable<IMemberValueConverter> Converters => ConverterList;


        /// <summary>
        /// Регистрирует <see cref="IMemberValueConverter"/>.
        /// </summary>
        public static void Register(IMemberValueConverter converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            if (!ConverterList.Contains(converter))
            {
                ConverterList.Add(converter);
            }
        }
    }
}