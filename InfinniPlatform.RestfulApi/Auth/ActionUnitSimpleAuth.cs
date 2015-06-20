using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.RestfulApi.Utils;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.RestfulApi.Auth
{
    /// <summary>
    ///     Модуль проверки авторизации
    /// </summary>
    public sealed class ActionUnitSimpleAuth
    {
        public void Action(IApplyContext target)
        {
            new AuthUtils(target.Context.GetComponent<ISecurityComponent>(target.Version), target.UserName, null)
                .CheckDocumentAccess(target.Item.Configuration, target.Item.Metadata, target.Item.Action,
                                     target.Item.RecordId);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = target.IsValid;
            //TODO связано с некорректной обработкой сообщений валидации (см класс StateTransition)
            //в дальнейшем target.ValidationMessage анализируется после выполнения валидации и там анализируются и сообщения авторизации
            //в том случае, если авторизация прошла успешно, что приводит к ошибке. Поэтому, в случае успеха проверки, возвращаем пустое сообщение
            target.Result.ValidationMessage = !target.IsValid ? target.ValidationMessage : null;
        }
    }
}