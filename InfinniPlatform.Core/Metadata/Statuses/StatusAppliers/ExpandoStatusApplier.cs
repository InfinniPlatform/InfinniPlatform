using System.Collections.Generic;
using System.Dynamic;
using InfinniPlatform.Api.DocumentStatus;

namespace InfinniPlatform.Metadata.Statuses.StatusAppliers
{
    class ExpandoStatusApplier : IStatusApplier
    {
        public bool TryApply(StatusVersion status, object target)
        {
            var expando = target as ExpandoObject;
            if (expando == null)
                return false;

            ((IDictionary<string, object>)expando)[status.Field] = status.Value;
            return true;
        }

        public bool CheckValue(StatusVersion status, object target)
        {
            var expando = target as ExpandoObject;
            if (expando == null)
                return false;

            return ((IDictionary<string, object>)expando)[status.Field] == status.Value;
        }
    }
}
