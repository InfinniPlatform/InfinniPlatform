using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Auth.Identity.MongoDb;
using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Producers;
using InfinniPlatform.Settings;

using Microsoft.Extensions.Caching.Memory;

namespace InfinniPlatform.Auth.UserStorage
{
    internal class AppUserStoreCache : IUserCacheSynchronizer
    {
        public AppUserStoreCache(UserStorageSettings userStorageSettings,
                                 ILog log,
                                 IBroadcastProducer broadcastProducer,
                                 AppOptions appOptions)
        {
            var cacheTimeout = userStorageSettings.UserCacheTimeout <= 0
                                   ? UserStorageSettings.DefaultUserCacheTimeout
                                   : userStorageSettings.UserCacheTimeout;

            _cacheTimeout = TimeSpan.FromMinutes(cacheTimeout);
            _log = log;
            _broadcastProducer = broadcastProducer;
            _appOptions = appOptions;

            _cacheLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            _usersById = new MemoryCache(new MemoryCacheOptions());
            _usersByName = new ConcurrentDictionary<string, IdentityUser>();
            _usersByEmail = new ConcurrentDictionary<string, IdentityUser>();
            _usersByPhone = new ConcurrentDictionary<string, IdentityUser>();
            _usersByLogin = new ConcurrentDictionary<string, IdentityUser>();
        }


        private readonly AppOptions _appOptions;
        private readonly IBroadcastProducer _broadcastProducer;

        private readonly ReaderWriterLockSlim _cacheLockSlim;
        private readonly TimeSpan _cacheTimeout;
        private readonly ILog _log;
        private readonly ConcurrentDictionary<string, IdentityUser> _usersByEmail;

        private readonly MemoryCache _usersById;
        private readonly ConcurrentDictionary<string, IdentityUser> _usersByLogin;
        private readonly ConcurrentDictionary<string, IdentityUser> _usersByName;
        private readonly ConcurrentDictionary<string, IdentityUser> _usersByPhone;


        /// <summary>
        /// Возвращает сведения о пользователе системы по его идентификатору.
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public IdentityUser FindUserById(string userId)
        {
            return GetUserCache(userId);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его имени.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public IdentityUser FindUserByUserName(string userName)
        {
            return GetAdditionalUserCache(_usersByName, userName);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public IdentityUser FindUserByEmail(string email)
        {
            return GetAdditionalUserCache(_usersByEmail, email);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его номеру телефона.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public IdentityUser FindUserByPhoneNumber(string phoneNumber)
        {
            return GetAdditionalUserCache(_usersByPhone, phoneNumber);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его имени у внешнего провайдера.
        /// </summary>
        /// <param name="userLogin">Имя входа пользователя системы у внешнего провайдера.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public IdentityUser FindUserByLogin(IdentityUserLogin userLogin)
        {
            return GetAdditionalUserCache(_usersByLogin, GetUserLoginKey(userLogin));
        }

        /// <summary>
        /// Добавляет или обновляет сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        public void AddOrUpdateUser(IdentityUser user)
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

                    NotifyOnUserChanged(user.Id);
                }
            }
            finally
            {
                _cacheLockSlim.ExitWriteLock();
            }
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


        //IUserCacheSynchronizer


        public Task ProcessMessage(Message<string> message)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (message.AppId == _appOptions.AppInstance)
                    {
                        //ignore own message
                    }
                    else
                    {
                        var userId = (string)message.GetBody();

                        if (!string.IsNullOrEmpty(userId))
                        {
                            RemoveUser(userId);
                        }
                    }
                }
                catch (Exception exception)
                {
                    _log.Error(exception);
                }
            });
        }

        private async void NotifyOnUserChanged(string userId)
        {
            await _broadcastProducer.PublishAsync(userId);
        }

        private static string GetUserLoginKey(IdentityUserLogin userLogin)
        {
            return $"{userLogin.LoginProvider},{userLogin.ProviderKey}";
        }

        private IdentityUser GetUserCache(string userId)
        {
            _cacheLockSlim.EnterReadLock();

            try
            {
                return (IdentityUser)_usersById.Get(userId);
            }
            finally
            {
                _cacheLockSlim.ExitReadLock();
            }
        }

        private void SetUserCache(string userId, IdentityUser user)
        {
            var absoluteExpiration = DateTimeOffset.Now.Add(_cacheTimeout);

            var options = new MemoryCacheEntryOptions
                          {
                              AbsoluteExpiration = absoluteExpiration,
                              PostEvictionCallbacks = {new PostEvictionCallbackRegistration {EvictionCallback = OnRemoveUserFromCache}}
                          };

            _usersById.Set(userId, user, options);
        }

        private void OnRemoveUserFromCache(object key, object value, EvictionReason reason, object state)
        {
            // TODO Use EvictionReason to filter?

            var removedUser = (IdentityUser)value;

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

        private void RemoveUserCache(string userId)
        {
            _usersById.Remove(userId);
        }

        private IdentityUser GetAdditionalUserCache(IDictionary<string, IdentityUser> additionalCache, string userKey)
        {
            _cacheLockSlim.EnterReadLock();

            try
            {
                IdentityUser user;
                additionalCache.TryGetValue(userKey, out user);
                return user;
            }
            finally
            {
                _cacheLockSlim.ExitReadLock();
            }
        }

        private static void SetAdditionalUserCache(IDictionary<string, IdentityUser> additionalCache, string userKey, IdentityUser user)
        {
            if (!string.IsNullOrEmpty(userKey))
            {
                additionalCache[userKey] = user;
            }
        }

        private static void RemoveAdditionalUserCache(IDictionary<string, IdentityUser> additionalCache, string userKey)
        {
            if (!string.IsNullOrEmpty(userKey))
            {
                additionalCache.Remove(userKey);
            }
        }
    }
}