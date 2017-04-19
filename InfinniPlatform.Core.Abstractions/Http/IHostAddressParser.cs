using System.Threading.Tasks;

namespace InfinniPlatform.Core.Http
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
        Task<bool> IsLocalAddress(string hostNameOrAddress);
    }
}