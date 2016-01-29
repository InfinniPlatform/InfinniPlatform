using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.ElasticSearch.Tests
{
    /// <summary>
    /// Создает шаблон документа для сохранения в тестах ElasticSearch.
    /// Нужно избавиться от этого класса после рефакторинга тестов и избавления от служебных полей типа TenantId и Status.
    /// </summary>
    public class ElasticDocument
    {
        public static dynamic Create()
        {
            dynamic template = new DynamicWrapper();
            template.TenantId = "anonimous";
            template.Status = "valid";
            template.Values = new DynamicWrapper();
            return template;
        }
    }
}