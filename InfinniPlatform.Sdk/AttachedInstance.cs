namespace InfinniPlatform.Sdk
{
    /// <summary>
    ///   Документ, присоединяемый к клиентской сессии
    /// </summary>
    public sealed class AttachedInstance
    {
        /// <summary>
        ///   Идентификатор приложения
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        ///   Идентификатор типа документа
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        ///  Присоединяемая сущность
        /// </summary>
        public dynamic Instance { get; set; }
    }
}
