namespace InfinniPlatform.UserInterface.ViewBuilders.Views
{
    /// <summary>
    ///     Дочерний элемент представления.
    /// </summary>
    public interface IViewChild
    {
        /// <summary>
        ///     Возвращает родительское представление.
        /// </summary>
        View GetView();
    }
}