using System.Reflection;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Обработчик разрешения зависимостей, передаваемых через параметры конструкторов.
    /// </summary>
    public interface IContainerParameterResolver
    {
        /// <summary>
        /// Определяет, какие параметры конструктора могут быть разрешены с помощью данного обработчика.
        /// </summary>
        /// <param name="parameterInfo">Информация о параметре конструктора.</param>
        /// <param name="resolver">Провайдер разрешения зависимостей.</param>
        bool CanResolve(ParameterInfo parameterInfo, IContainerResolver resolver);

        /// <summary>
        /// Разрешает значение для указанного параметра конструктора.
        /// </summary>
        /// <param name="parameterInfo">Информация о параметре конструктора.</param>
        /// <param name="resolver">Провайдер разрешения зависимостей.</param>
        object Resolve(ParameterInfo parameterInfo, IContainerResolver resolver);
    }
}