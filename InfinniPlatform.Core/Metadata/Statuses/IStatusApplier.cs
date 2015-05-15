using InfinniPlatform.Api.DocumentStatus;

namespace InfinniPlatform.Metadata.Statuses
{
    interface IStatusApplier
    {
        bool TryApply(StatusVersion status, object target);
        bool CheckValue(StatusVersion status, object target);
    }
}
