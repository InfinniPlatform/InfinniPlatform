using System.Collections.Generic;
using System.Text.RegularExpressions;

using InfinniPlatform.ElasticSearch.ElasticProviders;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.ElasticSearch.Filters.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Выражение для поиска в строке формата полей
        /// Полями считаются word-characters, заключенные в %
        /// </summary>
        private static readonly Regex FieldMatch = new Regex(@"(?!\\)%(\w|\.|\\%)+(?!\\)%");

        /// <summary>
        /// Создать на основе строки формата объект <see cref="ElasticScript"/>
        /// <para>Формат:</para>
        /// <para>%name% - имя поля документа</para>
        /// </summary>
        public static ICalculatedField ScriptFormat(this string format)
        {
            var result = format;
            var rawFields = FieldMatch.Matches(format);
            var fieldFactory = new ElasticScriptFactory();

            foreach (Match field in rawFields)
            {
                var scriptField = fieldFactory.Field(field.Value.Trim('%')).ToString();
                result = result.Replace(field.Value, scriptField);
            }

            return new ElasticScript(result);
        }

        /// <summary>
        /// Приведение имени поля к виду, понятному ElasticSearch
        /// </summary>
        public static string AsElasticField(this string fieldName)
        {
            var resultString = new List<string>();

            var fieldsSet = fieldName.Split('\n');

            foreach (var field in fieldsSet)
            {
                // Ко всем именам полей кроме TimeStamp необходимо добавить префикс Values.
                if (string.CompareOrdinal(field, ElasticConstants.IndexObjectTimeStampField) != 0 &&
                    !string.IsNullOrEmpty(field))
                {
                    resultString.Add(ElasticConstants.IndexObjectPath + field);
                }
            }

            return string.Join("\n", resultString);
        }
    }
}
