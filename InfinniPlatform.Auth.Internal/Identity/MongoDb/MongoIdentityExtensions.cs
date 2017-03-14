using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace InfinniPlatform.Auth.Internal.Identity.MongoDb
{
    public static class MongoIdentityExtensions
    {
//        public static IdentityBuilder AddMongoDbIdentity(this IServiceCollection services, string address, int port, string databaseName)
//        {
//            return services.AddIdentity<IdentityUser, IdentityRole>().RegisterMongoStores(address, port, databaseName);
//        }
//
//        private static IdentityBuilder RegisterMongoStores(this IdentityBuilder builder, string address, int port, string databaseName)
//        {
//            var mongoClientSettings = new MongoClientSettings
//                                      {
//                                          Servers = new[] {new MongoServerAddress(address, port)}
//                                      };
//
//            var mongoClient = new MongoClient(mongoClientSettings);
//
//            var database = mongoClient.GetDatabase(databaseName);
//
//            return builder.RegisterMongoStores(p => database.GetCollection<IdentityUser>("UserStore"), p => database.GetCollection<IdentityRole>("RoleStore"));
//        }
//
//        private static IdentityBuilder RegisterMongoStores<IdentityUser, IdentityRole>(this IdentityBuilder builder, Func<IServiceProvider, IMongoCollection<IdentityUser>> usersCollectionFactory, Func<IServiceProvider, IMongoCollection<IdentityRole>> rolesCollectionFactory)
//        {
//            if (typeof(IdentityUser) != builder.UserType)
//            {
//                throw new ArgumentException($"User type passed to RegisterMongoStores must match user type passed to AddIdentity. You passed {builder.UserType} to AddIdentity and {typeof(IdentityUser)} to RegisterMongoStores, these do not match.");
//            }
//
//            if (typeof(IdentityRole) != builder.RoleType)
//            {
//                throw new ArgumentException($"Role type passed to RegisterMongoStores must match role type passed to AddIdentity. You passed {builder.RoleType} to AddIdentity and {typeof(IdentityRole)} to RegisterMongoStores, these do not match.");
//            }
//
//            builder.Services.AddSingleton(p => (IUserStore<IdentityUser>) new UserStore<IdentityUser>(usersCollectionFactory(p)));
//            builder.Services.AddSingleton(p => (IRoleStore<IdentityRole>) new RoleStore<IdentityRole>(rolesCollectionFactory(p)));
//
//            return builder;
//        }
    }
}