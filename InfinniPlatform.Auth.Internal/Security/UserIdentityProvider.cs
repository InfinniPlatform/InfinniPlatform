using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading;

using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Auth.Internal.Security
{
    internal class UserIdentityProvider : IUserIdentityProvider
    {
        public IIdentity GetUserIdentity()
        {
            var requestUser = TryGetRequestUser();

            return requestUser?.Identity;
        }


        private const string RequestUserKey = "RequestUser";

        /// <summary>
        /// Устанавливает информацию об идентификационных данных текущего пользователя.
        /// </summary>
        /// <remarks>
        /// Во-первых, хранится только слабая ссылка на идентификационные данные текущего пользователя.
        /// Во-вторых, ссылка хранится в CallContext, чтобы она была доступна в любом месте обработки запроса,
        /// включая места распараллеливания через async/await. В-третьих, ссылка устанавливается перед началом
        /// обработки любого запроса.
        /// </remarks>
        public static void SetRequestUser(IPrincipal requestUser)
        {
            var requestUserReference = new WeakReference<IPrincipal>(requestUser);
            CallContext.LogicalSetData(RequestUserKey, requestUserReference);
            Thread.CurrentPrincipal = requestUser;
        }

        /// <summary>
        /// Возвращает информацию об идентификационных данных текущего пользователя, если она доступна.
        /// </summary>
        public static IPrincipal TryGetRequestUser()
        {
            IPrincipal requestUser;
            var requestUserReference = CallContext.LogicalGetData(RequestUserKey) as WeakReference<IPrincipal>;
            return (requestUserReference != null && requestUserReference.TryGetTarget(out requestUser)) ? requestUser : null;
        }
    }
}