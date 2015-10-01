using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Contracts
{
    /// <summary>
    ///   Контракт для разрешния зависимости пакета стандартных компонентов платформы
    /// </summary>
    public interface IPlatformComponentsPack
    {
        /// <summary>
        ///   Получить компонент платформы, реализующий указанный контракт
        /// </summary>
        /// <typeparam name="T">Тип контракта</typeparam>
        /// <returns>Экземпляр компонента</returns>
        T GetComponent<T>() where T : class;
    }
}
