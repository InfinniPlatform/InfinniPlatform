namespace InfinniPlatform.Sdk.RestApi
{
    public interface ICustomServiceApi
    {
        /// <summary>
        /// Выполнить вызов пользовательского сервиса
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        dynamic ExecuteAction(string application, string documentType, string service, dynamic body);
    }
}