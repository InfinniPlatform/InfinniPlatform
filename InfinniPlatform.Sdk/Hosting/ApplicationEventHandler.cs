namespace InfinniPlatform.Sdk.Hosting
{
    /// <summary>
    /// Базовый класс обработчика событий приложения.
    /// </summary>
    public abstract class ApplicationEventHandler : IApplicationEventHandler
    {
        protected ApplicationEventHandler(int order = 0)
        {
            Order = order;
        }

        /// <summary>
        /// Порядковый номер при выполнении.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Обрабатывает событие запуска приложения.
        /// </summary>
        public virtual void OnStart()
        {
        }

        /// <summary>
        /// Обрабатывает событие остановки приложения.
        /// </summary>
        public virtual void OnStop()
        {
        }
    }
}