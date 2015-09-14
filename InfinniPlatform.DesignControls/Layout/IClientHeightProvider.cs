namespace InfinniPlatform.DesignControls.Layout
{
    public interface IClientHeightProvider
    {
        /// <summary>
        ///     Получить размер клиентской области, занимаемой контролом
        /// </summary>
        /// <returns></returns>
        int GetClientHeight();

        /// <summary>
        ///     Признак фиксированного (не вычисляемого) размера контрола
        /// </summary>
        /// <returns></returns>
        bool IsFixedHeight();
    }
}