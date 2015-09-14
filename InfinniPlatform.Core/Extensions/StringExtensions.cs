using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Выполянет string.Format для указанной строки формата и набора параметров
        /// </summary>
        public static string StringFormat(this string format, params object[] values)
        {
            return String.Format(format, values);
        }

        /// <summary>
        /// Выполянет транслитерацию для указанной строки
        /// </summary>
        public static string ToTranslit(this string source)
        {
            foreach (var c in source.Where(c => !Char.IsLetterOrDigit(c) && c != '_'))
            {
                source = source.Replace(c, '_');
            }

            var translated = Words.Aggregate(source, (current, word) => current.Replace(word.Key, word.Value));
            translated = translated.TrimStart(' ')
                                   .TrimEnd('_');

            while (translated.Contains("__"))
            {
                translated = translated.Replace("__", "_");
            }

            return translated;
        }

        private static readonly Dictionary<string, string> Words = new Dictionary<string, string>
            {
                {"а", "a"},
                {"б", "b"},
                {"в", "v"},
                {"г", "g"},
                {"д", "d"},
                {"е", "e"},
                {"ё", "yo"},
                {"ж", "zh"},
                {"з", "z"},
                {"и", "i"},
                {"й", "j"},
                {"к", "k"},
                {"л", "l"},
                {"м", "m"},
                {"н", "n"},
                {"о", "o"},
                {"п", "p"},
                {"р", "r"},
                {"с", "s"},
                {"т", "t"},
                {"у", "u"},
                {"ф", "f"},
                {"х", "h"},
                {"ц", "c"},
                {"ч", "ch"},
                {"ш", "sh"},
                {"щ", "sch"},
                {"ъ", "j"},
                {"ы", "i"},
                {"ь", "j"},
                {"э", "e"},
                {"ю", "yu"},
                {"я", "ya"},
                {"А", "A"},
                {"Б", "B"},
                {"В", "V"},
                {"Г", "G"},
                {"Д", "D"},
                {"Е", "E"},
                {"Ё", "Yo"},
                {"Ж", "Zh"},
                {"З", "Z"},
                {"И", "I"},
                {"Й", "J"},
                {"К", "K"},
                {"Л", "L"},
                {"М", "M"},
                {"Н", "N"},
                {"О", "O"},
                {"П", "P"},
                {"Р", "R"},
                {"С", "S"},
                {"Т", "T"},
                {"У", "U"},
                {"Ф", "F"},
                {"Х", "H"},
                {"Ц", "C"},
                {"Ч", "Ch"},
                {"Ш", "Sh"},
                {"Щ", "Sch"},
                {"Ъ", "J"},
                {"Ы", "I"},
                {"Ь", "J"},
                {"Э", "E"},
                {"Ю", "Yu"},
                {"Я", "Ya"},
            {" ", "_"}
            };
    }
}
