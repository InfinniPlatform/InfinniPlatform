using System;
using System.Security.Principal;
using System.Threading;

namespace InfinniPlatform.Security
{
    internal class UserIdentityProvider : IUserIdentityProvider
    {
        private static readonly AsyncLocal<WeakReference<IPrincipal>> RequestUserReference = new AsyncLocal<WeakReference<IPrincipal>>();

        public IIdentity GetUserIdentity()
        {
            var requestUser = TryGetRequestUser();

            return requestUser?.Identity;
        }

        public void SetUserIdentity(IPrincipal identity)
        {
            SetRequestUser(identity);
        }


        /// <summary>
        /// Возвращает идентификационные данные текущего пользователя.
        /// </summary>
        private static IPrincipal TryGetRequestUser()
        {
            IPrincipal requestUser;

            return RequestUserReference.Value != null && RequestUserReference.Value.TryGetTarget(out requestUser) ? requestUser : null;
        }

        /// <summary>
        /// Устанавливает идентификационные данные текущего пользователя.
        /// </summary>
        /// <remarks>
        /// Во-первых, хранится только слабая ссылка на идентификационные данные текущего пользователя.
        /// Во-вторых, ссылка хранится в CallContext, чтобы она была доступна в любом месте обработки запроса,
        /// включая места распараллеливания через async/await. В-третьих, ссылка устанавливается перед началом
        /// обработки любого запроса.
        /// </remarks>
        private static void SetRequestUser(IPrincipal requestUser)
        {
            RequestUserReference.Value=new WeakReference<IPrincipal>(requestUser);
        }
    }
}