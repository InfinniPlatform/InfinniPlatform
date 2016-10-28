namespace InfinniPlatform.Sdk.Http
{
    /// <summary>
    /// Интерфейс для разбора адресов узлов.
    /// </summary>
    public interface IHostAddressParser
    {
        /// <summary>
        /// Определяет, является ли адрес локальным.
        /// </summary>
        /// <param name="hostNameOrAddress">Имя узла или его адрес.</param>
        /// <returns>Возвращает <c>true</c>, если адрес является локальным; иначе возвращает <c>false</c>.</returns>
        bool IsLocalAddress(string hostNameOrAddress);

        /// <summary>
        /// Определяет, является ли адрес локальным.
        /// </summary>
        /// <param name="hostNameOrAddress">Имя узла или его адрес.</param>
        /// <param name="normalizedAddress">Нормализованный адрес узла.</param>
        /// <returns>Возвращает <c>true</c>, если адрес является локальным; иначе возвращает <c>false</c>.</returns>
        bool IsLocalAddress(string hostNameOrAddress, out string normalizedAddress);
    }
}