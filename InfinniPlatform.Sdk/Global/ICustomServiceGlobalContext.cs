using System;

using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Sdk.Global
{
    /// <summary>
    ///   Глобальный контекст пользовательского сервиса
    /// </summary>
    [Obsolete("Use IoC")]
    public interface ICustomServiceGlobalContext : IGlobalContext
    {
    }
}
