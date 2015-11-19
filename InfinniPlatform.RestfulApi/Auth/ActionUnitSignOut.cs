using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
    public sealed class ActionUnitSignOut
    {
        public void Action(IApplyContext target)
        {
            // TODO: Сервис SignInApi был удален ввиду своей неактуальности.

            target.Result = new DynamicWrapper();
            target.Result.IsValid = false;
            target.Result.ValidationMessage = "Not Supported";
        }
    }
}