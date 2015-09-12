using System;

namespace InfinniPlatform.Api.SelfDocumentation
{
    /// <summary>
    ///     Атрибут, используемый для юнит-тестов.
    ///     Позволяет генерировать документацию по юнит-тестам
    /// </summary>
    public class SelfDocumentedTestAttribute : Attribute
    {
        public SelfDocumentedTestAttribute()
        {
            Comment = string.Empty;
        }

        public string Comment { get; set; }
    }
}