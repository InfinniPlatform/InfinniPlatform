using System.Reflection;
using InfinniPlatform.Api.DocumentStatus;

namespace InfinniPlatform.Metadata.Statuses.StatusAppliers
{
    class FieldStatusApplier : IStatusApplier
    {
        public bool TryApply(StatusVersion status, object target)
        {
            var targetType = target.GetType();
            const BindingFlags fieldFlag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

            var asField = targetType.GetField(status.Field, fieldFlag);
            if (asField == null)
                return false;

            asField.SetValue(target, status.Value);
            return true;
        }

        public bool CheckValue(StatusVersion status, object target)
        {
            var targetType = target.GetType();
            const BindingFlags fieldFlag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

            var asField = targetType.GetField(status.Field, fieldFlag);
            if (asField == null)
                return false;

            return asField.GetValue(target) == status.Value;
        }
    }
}
