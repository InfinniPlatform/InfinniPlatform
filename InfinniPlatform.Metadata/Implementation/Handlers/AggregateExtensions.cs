using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
    public static class AggregateExtensions
    {
        public static dynamic PrepareInvalidFilterAggregate(ICommonContext target)
        {
            dynamic result = new DynamicWrapper();
            result.IsValid = target.IsValid;
            result.IsInternalServerError = target.IsInternalServerError;
            result.ValidationMessage = target.ValidationMessage;
            return result;
        }

        public static dynamic PrepareAggregateNotFound(dynamic target)
        {
            dynamic response = new DynamicWrapper();
            response.ValidationMessage = "Aggregate not found";
            response.IsValid = false;
            response.Id = DynamicWrapperExtensions.ToDynamic(target).Id;
            return response;
        }

        public static dynamic PrepareResultAggregate(dynamic result)
        {
            return result;
        }

        public static object PrepareInvalidResultAggregate(ICommonContext targetResult)
        {
            dynamic response = new DynamicWrapper();
            response.ValidationMessage = targetResult.ValidationMessage;
            response.IsInternalServerError = targetResult.IsInternalServerError;
            response.IsValid = false;
            return response;
        }
    }
}