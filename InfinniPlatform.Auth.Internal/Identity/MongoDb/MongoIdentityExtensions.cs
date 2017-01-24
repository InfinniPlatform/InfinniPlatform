using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace InfinniPlatform.Auth.Internal.Identity.MongoDb
{
    public static class MongoIdentityExtensions
    {
        public static IdentityBuilder AddMongoDbIdentity(this IServiceCollection services, string address, int port, string databaseName)
        {
            return services.AddMongoDbIdentity<IdentityUser, IdentityRole>(address, port, databaseName);
        }

        public static IdentityBuilder AddMongoDbIdentity<TUser, TRole>(this IServiceCollection services, string address, int port, string databaseName) where TUser : IdentityUser where TRole : IdentityRole
        {
            return services.AddIdentity<TUser, TRole>().RegisterMongoStores<TUser, TRole>(address, port, databaseName);
        }

        private static IdentityBuilder RegisterMongoStores<TUser, TRole>(this IdentityBuilder builder, string address, int port, string databaseName) where TUser : IdentityUser where TRole : IdentityRole
        {
            var mongoClientSettings = new MongoClientSettings
                                      {
                                          Servers = new[] {new MongoServerAddress(address, port)}
                                      };

            var mongoClient = new MongoClient(mongoClientSettings);

            var database = mongoClient.GetDatabase(databaseName);

            return builder.RegisterMongoStores(p => database.GetCollection<TUser>("UserStore"), p => database.GetCollection<TRole>("RoleStore"));
        }

        private static IdentityBuilder RegisterMongoStores<TUser, TRole>(this IdentityBuilder builder, Func<IServiceProvider, IMongoCollection<TUser>> usersCollectionFactory, Func<IServiceProvider, IMongoCollection<TRole>> rolesCollectionFactory) where TUser : IdentityUser where TRole : IdentityRole
        {
            if (typeof(TUser) != builder.UserType)
            {
                throw new ArgumentException($"User type passed to RegisterMongoStores must match user type passed to AddIdentity. You passed {builder.UserType} to AddIdentity and {typeof(TUser)} to RegisterMongoStores, these do not match.");
            }

            if (typeof(TRole) != builder.RoleType)
            {
                throw new ArgumentException($"Role type passed to RegisterMongoStores must match role type passed to AddIdentity. You passed {builder.RoleType} to AddIdentity and {typeof(TRole)} to RegisterMongoStores, these do not match.");
            }

            builder.Services.AddSingleton(p => (IUserStore<TUser>) new UserStore<TUser>(usersCollectionFactory(p)));
            builder.Services.AddSingleton(p => (IRoleStore<TRole>) new RoleStore<TRole>(rolesCollectionFactory(p)));

            return builder;
        }
    }
}