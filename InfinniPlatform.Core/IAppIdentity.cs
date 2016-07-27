namespace InfinniPlatform.Core
{
    /// <summary>
    /// Позволяет идентифицировать текущий экземпляр приложения.
    /// </summary>
    /// <remarks>
    /// При работе с несколькими экземплярами одного приложения.
    /// </remarks>
    public interface IAppIdentity
    {
        /// <summary>
        /// Идентификатор текущего экземпляра приложения.
        /// </summary>
        string Id { get; }
    }
}