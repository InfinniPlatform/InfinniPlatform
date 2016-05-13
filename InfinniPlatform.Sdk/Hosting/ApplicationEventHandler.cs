namespace InfinniPlatform.Sdk.Hosting
{
    /// <summary>
    /// Базовый класс обработчика событий приложения.
    /// </summary>
    public abstract class 
        ApplicationEventHandler : IApplicationEventHandler
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
        /// Вызывается перед запуском приложения.
        /// </summary>
        public virtual void OnBeforeStart()
        {
        }

        /// <summary>
        /// Вызывается после запуска приложения.
        /// </summary>
        public virtual void OnAfterStart()
        {
        }

        /// <summary>
        /// Вызывается перед остановкой приложения.
        /// </summary>
        public virtual void OnBeforeStop()
        {
        }

        /// <summary>
        /// Вызывается после остановки приложения.
        /// </summary>
        public virtual void OnAfterStop()
        {
        }
    }
}