using System;
using System.Globalization;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.Extensions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
    /// <summary>
    /// Вспомогательный класс для формировоания выражения с использованием скриптового языка Elastic Search.
    /// Позволяет формировать скрипт в следущей форме:
    /// ElasticScript.Field("rating") * ElasticScript.Const(10) / ElasticScript.Field("countOfStudents")
    /// </summary>
    public class ElasticScript : ICalculatedField
    {
        private readonly string _scriptRepresentation;

        internal ElasticScript(string script)
        {
            _scriptRepresentation = script;
        }

        /// <summary>
        /// Возвращает текст скрипта
        /// </summary>
        public override string ToString()
        {
            return _scriptRepresentation;
        }

        /// <summary>
        ///   Получить код скрипта для исполнения
        /// </summary>
        /// <returns>Код скрипта</returns>
        public string GetRawScript()
        {
            return ToString();
        }

        public ICalculatedField Add(ICalculatedField item)
        {
            return new ElasticScript(string.Format("({0} + {1})", this, item));
        }

        public ICalculatedField Subtract(ICalculatedField item)
        {
            return new ElasticScript(string.Format("({0} - {1})", this, item));
        }

        public ICalculatedField Divide(ICalculatedField item)
        {
            return new ElasticScript(string.Format("({0} / {1})", this, item));
        }

        public ICalculatedField Multiply(ICalculatedField item)
        {
            return new ElasticScript(string.Format("({0} * {1})", this, item));
        }
    }


    public class ElasticScriptFactory : ICalculatedFieldFactory
    {
        /// <summary>
        /// Обращение к полю в ElasticScript выглядит так: doc['field_name'].value
        /// </summary>
        private const string FieldNameRepresentationPattern = "doc['{0}'].value";

        private const int MsInDay = 24 * 60 * 60 * 1000;

        public ICalculatedField Const(double item)
        {
            return new ElasticScript(item.AsElasticValue());
        }

        public ICalculatedField Const(int item)
        {
            return new ElasticScript(item.ToString(new CultureInfo("en-US")));
        }

        public ICalculatedField RawString(string item)
        {
            return new ElasticScript(item);
        }

        public ICalculatedField DateTrimTime(string fieldName)
        {
            return new ElasticScript(String.Format("( floor({0} / {1}) * {1} )", Field(fieldName), MsInDay));
        }

        public ICalculatedField Field(string fieldName)
        {
            return new ElasticScript(string.Format(FieldNameRepresentationPattern, fieldName.AsElasticField()));
        }
    }
}