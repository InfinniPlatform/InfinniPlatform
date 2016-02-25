namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Фильтр, поддерживающий логические операции И, ИЛИ, отрицание
    /// </summary>
    public interface IFilter
    {
        IFilter And(IFilter target);
        IFilter Or(IFilter target);
        IFilter Not();
    }


    /// <summary>
    /// Расширение интерфейса <see cref="IFilter" /> с возможностью получить экземпляр объекта, использующегося для
    /// фильтрации в более низком уровне абстракции
    /// </summary>
    public interface IFilter<out T> : IFilter
    {
        T GetFilterObject();
    }
}