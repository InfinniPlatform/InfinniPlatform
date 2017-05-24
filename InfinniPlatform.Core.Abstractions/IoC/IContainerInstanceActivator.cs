using System;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Обработчик инициализации экземпляра зависимости.
    /// </summary>
    public interface IContainerInstanceActivator
    {
        /// <summary>
        /// Инициирует экземпляр зависимости.
        /// </summary>
        /// <param name="instance">Экземпляр зависимости.</param>
        /// <param name="resolver">Провайдер разрешения зависимостей.</param>
        void Activate(object instance, Func<IContainerResolver> resolver);
    }
}