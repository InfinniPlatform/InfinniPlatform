namespace InfinniPlatform.Sdk.Http.Services
{
    public static class HttpConstants
    {
        /// <summary>
        /// Размер буфера для передачи файлов (4 Мб).
        /// </summary>
        public const int FileBufferSize = 4 * 1024 * 1024;

        /// <summary>
        /// Тип содержимого 'application/pdf'.
        /// </summary>
        public const string PdfContentType = "application/pdf";

        /// <summary>
        /// Тип содержимого 'text/html'.
        /// </summary>
        public const string HtmlContentType = "text/html";

        /// <summary>
        /// Тип содержимого 'text/plain'.
        /// </summary>
        public const string TextPlainContentType = "text/plain";

        /// <summary>
        /// Тип содержимого 'application/json'.
        /// </summary>
        public const string JsonContentType = "application/json";

        /// <summary>
        /// Тип содержимого 'application/text'.
        /// </summary>
        public const string TextContentType = "application/text";

        /// <summary>
        /// Тип содержимого "application/octet-stream".
        /// </summary>
        public const string StreamContentType = "application/octet-stream";

        /// <summary>
        /// Тип содержимого "multipart/form-data".
        /// </summary>
        public const string MultipartFormDataContentType = "multipart/form-data";

        /// <summary>
        /// Тип содержимого "application/x-www-form-urlencoded".
        /// </summary>
        public const string FormUrlencodedContentType = "application/x-www-form-urlencoded";
    }
}