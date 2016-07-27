using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Caching.RabbitMQ;
using InfinniPlatform.Core;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Queues.Producers;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Authentication.UserStorage
{
    internal sealed class ApplicationUserStoreCache : BroadcastConsumerBase<UserCacheMessage>
    {
        public ApplicationUserStoreCache(UserStorageSettings userStorageSettings, ILog log, IBroadcastProducer broadcastProducer, IAppIdentity appIdentity)
        {
            var cacheTimeout = userStorageSettings.UserCacheTimeout <= 0
                                   ? UserStorageSettings.DefaultUserCacheTimeout
                                   : userStorageSettings.UserCacheTimeout;

            _cacheTimeout = TimeSpan.FromMinutes(cacheTimeout);
            _log = log;
            _broadcastProducer = broadcastProducer;
            _appIdentity = appIdentity;

            _cacheLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            _usersById = new MemoryCache("UserCache");
            _usersByName = new ConcurrentDictionary<string, ApplicationUser>();
            _usersByEmail = new ConcurrentDictionary<string, ApplicationUser>();
            _usersByPhone = new ConcurrentDictionary<string, ApplicationUser>();
            _usersByLogin = new ConcurrentDictionary<string, ApplicationUser>();
        }

        private readonly IAppIdentity _appIdentity;
        private readonly IBroadcastProducer _broadcastProducer;

        private readonly ReaderWriterLockSlim _cacheLockSlim;

        private readonly TimeSpan _cacheTimeout;
        private readonly ILog _log;
        private readonly ConcurrentDictionary<string, ApplicationUser> _usersByEmail;

        private readonly MemoryCache _usersById;
        private readonly ConcurrentDictionary<string, ApplicationUser> _usersByLogin;
        private readonly ConcurrentDictionary<string, ApplicationUser> _usersByName;
        private readonly ConcurrentDictionary<string, ApplicationUser> _usersByPhone;

        /// <summary>
        /// Возвращает сведения о пользователе системы по его идентификатору.
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public ApplicationUser FindUserById(string userId)
        {
            return GetUserCache(userId);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его имени.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public ApplicationUser FindUserByUserName(string userName)
        {
            return GetAdditionalUserCache(_usersByName, userName);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public ApplicationUser FindUserByEmail(string email)
        {
            return GetAdditionalUserCache(_usersByEmail, email);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его номеру телефона.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public ApplicationUser FindUserByPhoneNumber(string phoneNumber)
        {
            return GetAdditionalUserCache(_usersByPhone, phoneNumber);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его имени у внешнего провайдера.
        /// </summary>
        /// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public ApplicationUser FindUserByLogin(ApplicationUserLogin userLogin)
        {
            return GetAdditionalUserCache(_usersByLogin, GetUserLoginKey(userLogin));
        }

        /// <summary>
        /// Добавляет или обновляет сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        public void AddOrUpdateUser(ApplicationUser user)
        {
            _cacheLockSlim.EnterWriteLock();

            try
            {
                SetUserCache(user.Id, user);
                SetAdditionalUserCache(_usersByName, user.UserName, user);
                SetAdditionalUserCache(_usersByEmail, user.Email, user);
                SetAdditionalUserCache(_usersByPhone, user.PhoneNumber, user);

                var userLogins = user.Logins;

                if (userLogins != null)
                {
                    foreach (var userLogin in userLogins)
                    {
                        SetAdditionalUserCache(_usersByLogin, GetUserLoginKey(userLogin), user);
                    }
                }
            }
            finally
            {
                _cacheLockSlim.ExitWriteLock();
            }

            NotifyUserChanged(user.Id);
        }

        /// <summary>
        /// Удаляет сведения о пользователе системы.
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователе системы.</param>
        public void RemoveUser(string userId)
        {
            _cacheLockSlim.EnterWriteLock();

            try
            {
                RemoveUserCache(userId);
            }
            finally
            {
                _cacheLockSlim.ExitWriteLock();
            }
        }

        private void OnRemoveUserFromCache(CacheEntryRemovedArguments args)
        {
            var removedUser = (ApplicationUser)args.CacheItem.Value;

            _cacheLockSlim.EnterWriteLock();

            try
            {
                RemoveAdditionalUserCache(_usersByName, removedUser.UserName);
                RemoveAdditionalUserCache(_usersByEmail, removedUser.Email);
                RemoveAdditionalUserCache(_usersByPhone, removedUser.PhoneNumber);

                var userLogins = removedUser.Logins;

                if (userLogins != null)
                {
                    foreach (var userLogin in userLogins)
                    {
                        RemoveAdditionalUserCache(_usersByLogin, GetUserLoginKey(userLogin));
                    }
                }
            }
            finally
            {
                _cacheLockSlim.ExitWriteLock();
            }
        }

        private void NotifyUserChanged(string userId)
        {
            // Оповещаем другие узлы об изменении сведений пользователя

            _broadcastProducer.Publish(new UserCacheMessage(userId));
        }

        private static string GetUserLoginKey(ApplicationUserLogin userLogin)
        {
            return $"{userLogin.Provider},{userLogin.ProviderKey}";
        }

        private ApplicationUser GetUserCache(string userId)
        {
            _cacheLockSlim.EnterReadLock();

            try
            {
                return (ApplicationUser)_usersById.Get(userId);
            }
            finally
            {
                _cacheLockSlim.ExitReadLock();
            }
        }

        private void SetUserCache(string userId, ApplicationUser user)
        {
            var absoluteExpiration = DateTimeOffset.Now.Add(_cacheTimeout);

            _usersById.Set(userId, user, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration, RemovedCallback = OnRemoveUserFromCache });
        }

        private void RemoveUserCache(string userId)
        {
            _usersById.Remove(userId);
        }

        private ApplicationUser GetAdditionalUserCache(IDictionary<string, ApplicationUser> additionalCache, string userKey)
        {
            _cacheLockSlim.EnterReadLock();

            try
            {
                ApplicationUser user;
                additionalCache.TryGetValue(userKey, out user);
                return user;
            }
            finally
            {
                _cacheLockSlim.ExitReadLock();
            }
        }

        private static void SetAdditionalUserCache(IDictionary<string, ApplicationUser> additionalCache, string userKey, ApplicationUser user)
        {
            if (!string.IsNullOrEmpty(userKey))
            {
                additionalCache[userKey] = user;
            }
        }

        private static void RemoveAdditionalUserCache(IDictionary<string, ApplicationUser> additionalCache, string userKey)
        {
            if (!string.IsNullOrEmpty(userKey))
            {
                additionalCache.Remove(userKey);
            }
        }

        protected override async Task Consume(Message<UserCacheMessage> message)
        {
            await Task.Run(() =>
                           {
                               try
                               {
                                   if (message.PublisherId == _appIdentity.Id)
                                   {
                                       //ignore own message
                                   }
                                   else
                                   {
                                       var userCacheMessage = (UserCacheMessage)message.GetBody();

                                       if (!string.IsNullOrEmpty(userCacheMessage.UserId))
                                       {
                                           RemoveUser(userCacheMessage.UserId);
                                       }
                                   }
                               }
                               catch (Exception e)
                               {
                                   _log.Error(e);
                               }
                           });
        }
    }
}