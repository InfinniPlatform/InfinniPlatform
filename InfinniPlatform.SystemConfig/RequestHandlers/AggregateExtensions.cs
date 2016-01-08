using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    internal static class AggregateExtensions
    {
        public static dynamic PrepareInvalidResult(ICommonContext target)
        {
            dynamic result = new DynamicWrapper();
            result.IsValid = false;
            result.IsInternalServerError = target.IsInternalServerError ? true : (bool?)null;
            result.ValidationMessage = target.ValidationMessage;
            return result;
        }
    }
}