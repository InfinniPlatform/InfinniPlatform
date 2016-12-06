using System.Collections;

namespace Infinni.Agent.Providers
{
    /// <summary>
    /// Предоставляет доступ к переменным окружения.
    /// </summary>
    public interface IEnvironmentVariableProvider
    {
        /// <summary>
        /// Возвращает список переменных окружения.
        /// </summary>
        IDictionary GetAll();

        /// <summary>
        /// Возвращает переменную окружения по имени.
        /// </summary>
        /// <param name="name">Имя переменной окружения.</param>
        IDictionary Get(string name);
    }
}