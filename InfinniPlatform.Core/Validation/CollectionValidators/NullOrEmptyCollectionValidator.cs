using System.Linq;

namespace InfinniPlatform.Core.Validation.CollectionValidators
{
    /// <summary>
    ///     Коллекция является нулевым указателем или пустой коллекцией.
    /// </summary>
    public sealed class NullOrEmptyCollectionValidator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            if (validationObject != null)
            {
                var collection = validationObject.TryCastToEnumerable();
                return (collection == null) || !collection.Any();
            }

            return true;
        }
    }
}