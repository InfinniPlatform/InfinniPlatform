namespace InfinniPlatform.MetadataDesigner.Views.Validation
{
    /// <summary>
    /// Контрол для формирования одного предиката валидационного выражения
    /// </summary>
    public interface IPredicateBuilderControl
    {
        /// <summary>
        /// Получить результат моделирования одного предиката
        /// </summary>
        PredicateDescriptionNode GetPredicateDescription();

        /// <summary>
        /// Получить следующий контрол для моделирвоания предиката
        /// </summary>
        PredicateDescriptionType GetNextControlType();
    }
}