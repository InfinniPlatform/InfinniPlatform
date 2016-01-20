namespace InfinniPlatform.Sdk.Services
{
    public static class HttpConstants
    {
        /// <summary>
        /// Размер буфера для передачи файлов (4 Мб).
        /// </summary>
        public static int FileBufferSize = 4 * 1024 * 1024;

        /// <summary>
        /// Тип содержимого 'application/pdf'.
        /// </summary>
        public const string PdfContentType = "application/pdf";

        /// <summary>
        /// Тип содержимого 'text/plain'.
        /// </summary>
        public const string TextContentType = "text/plain";

        /// <summary>
        /// Тип содержимого 'application/json'.
        /// </summary>
        public const string JsonContentType = "application/json";

        /// <summary>
        /// Тип содержимого "application/octet-stream".
        /// </summary>
        public const string StreamContentType = "application/octet-stream";
    }
}