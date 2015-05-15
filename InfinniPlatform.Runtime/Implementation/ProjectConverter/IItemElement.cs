namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    interface IItemElement : IProjectComponent
    {
        /// <summary>
        /// Тэг, который характеризует данный тип ресурсов
        /// </summary>
        string XmlElementTagName { get; }
    }
}
