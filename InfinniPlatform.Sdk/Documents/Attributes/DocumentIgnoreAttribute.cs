using System;

namespace InfinniPlatform.Sdk.Documents.Attributes
{
    /// <summary>
    /// Определяет, что свойство или член документа должен игнорироваться хранилищем документов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DocumentIgnoreAttribute : Attribute
    {
    }
}