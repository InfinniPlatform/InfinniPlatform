using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders.Data
{
    /// <summary>
    ///     Привязка элемента представления к источнику данных.
    /// </summary>
    public interface IElementDataBinding : IViewChild
    {
        /// <summary>
        ///     Возвращает или устанавливает обработчик события изменения значения в источнике данных.
        /// </summary>
        /// <remarks>
        ///     Обработчик устанавливает элемент представления для получения уведомлений об изменениях значения в источнике данных.
        /// </remarks>
        ScriptDelegate OnPropertyValueChanged { get; set; }

        /// <summary>
        ///     Устанавливает значение в источнике данных.
        /// </summary>
        /// <remarks>
        ///     Вызывает элемент представления для оповещения источника данных об изменениях.
        /// </remarks>
        void SetPropertyValue(object value, bool force = false);
    }
}