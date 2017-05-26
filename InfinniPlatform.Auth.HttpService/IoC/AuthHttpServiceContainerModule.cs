using InfinniPlatform.Http;
using InfinniPlatform.IoC;

namespace InfinniPlatform.Auth.HttpService.IoC
{
    public class AuthHttpServiceContainerModule<TUser> : IContainerModule where TUser : AppUser
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType(typeof(AuthInternalHttpService<>).MakeGenericType(typeof(TUser)))
                   .As<IHttpService>()
                   .SingleInstance();

            builder.RegisterType(typeof(AuthExternalHttpService<>).MakeGenericType(typeof(TUser)))
                .As<IHttpService>()
                .SingleInstance();

            builder.RegisterType(typeof(AuthManagementHttpService<>).MakeGenericType(typeof(TUser)))
                .As<IHttpService>()
                .SingleInstance();

            builder.RegisterType<UserEventHandlerInvoker>()
                   .AsSelf()
                   .SingleInstance();
        }
    }
}