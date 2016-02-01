namespace InfinniPlatform.Core.Documents
{
    public sealed class DocumentExecutorResult
    {
        public object Id { get; set; }

        public bool IsValid { get; set; }

        public object ValidationMessage { get; set; }

        public object IsInternalServerError { get; set; }

        //TODO: Костыль, пока не стандартизуем запросы/ответы к API.
        public object Result { get; set; }
    }
}