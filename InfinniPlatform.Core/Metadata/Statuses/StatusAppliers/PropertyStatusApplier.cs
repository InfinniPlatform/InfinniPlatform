using System.Reflection;
using InfinniPlatform.Api.DocumentStatus;

namespace InfinniPlatform.Metadata.Statuses.StatusAppliers
{
    class PropertyStatusApplier : IStatusApplier
    {
        public bool TryApply(StatusVersion status, object target)
        {
            var targetType = target.GetType();
            const BindingFlags fieldFlag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

            var asProperty = targetType.GetProperty(status.Field, fieldFlag);
            if (asProperty == null)
                return false;

            asProperty.SetValue(target, status.Value);
            return true;
        }

        public bool CheckValue(StatusVersion status, object target)
        {
            var targetType = target.GetType();
            const BindingFlags fieldFlag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

            var asProperty = targetType.GetProperty(status.Field, fieldFlag);
            if (asProperty == null)
                return false;

            return asProperty.GetValue(target) == status.Value;
        }
    }
}
