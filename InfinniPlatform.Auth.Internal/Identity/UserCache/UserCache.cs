using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserStorage;
using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue;
using Microsoft.Extensions.Caching.Memory;

namespace InfinniPlatform.Auth.Identity.UserCache
{
    public class UserCache<TUser> : IUserCacheObserver where TUser: IdentityUser
    {
        public UserCache(AuthInternalOptions options,
                         ILog log,
                         IBroadcastProducer broadcastProducer,
                         AppOptions appOptions)
        {
            var cacheTimeout = options.UserCacheTimeout <= 0
                                   ? AuthInternalOptions.DefaultUserCacheTimeout
                                   : options.UserCacheTimeout;

            _cacheTimeout = TimeSpan.FromMinutes(cacheTimeout);
            _log = log;
            _broadcastProducer = broadcastProducer;
            _appOptions = appOptions;

            _cacheLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            _usersById = new MemoryCache(new MemoryCacheOptions());
            _usersByName = new ConcurrentDictionary<string, TUser>();
            _usersByEmail = new ConcurrentDictionary<string, TUser>();
            _usersByPhone = new ConcurrentDictionary<string, TUser>();
            _usersByLogin = new ConcurrentDictionary<string, TUser>();
        }


        private readonly AppOptions _appOptions;
        private readonly IBroadcastProducer _broadcastProducer;

        private readonly ReaderWriterLockSlim _cacheLockSlim;
        private readonly TimeSpan _cacheTimeout;
        private readonly ILog _log;
        private readonly ConcurrentDictionary<string, TUser> _usersByEmail;

        private readonly MemoryCache _usersById;
        private readonly ConcurrentDictionary<string, TUser> _usersByLogin;
        private readonly ConcurrentDictionary<string, TUser> _usersByName;
        private readonly ConcurrentDictionary<string, TUser> _usersByPhone;


        /// <summary>
        /// Возвращает сведения о пользователе системы по его идентификатору.
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public TUser FindUserById(string userId)
        {
            return GetUserCache(userId);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его имени.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public TUser FindUserByUserName(string userName)
        {
            return GetAdditionalUserCache(_usersByName, userName);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его электронной почте.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public TUser FindUserByEmail(string email)
        {
            return GetAdditionalUserCache(_usersByEmail, email);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его номеру телефона.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона пользователя.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public TUser FindUserByPhoneNumber(string phoneNumber)
        {
            return GetAdditionalUserCache(_usersByPhone, phoneNumber);
        }

        /// <summary>
        /// Возвращает сведения о пользователе системы по его имени у внешнего провайдера.
        /// </summary>
        /// <param name="loginProvider">Внешний провайдер.</param>
        /// <param name="providerKey">Ключ внешнего провайдера.</param>
        /// <returns>Сведения о пользователе системы.</returns>
        public TUser FindUserByLogin(string loginProvider, string providerKey)
        {
            return GetAdditionalUserCache(_usersByLogin, GetUserLoginKey(loginProvider, providerKey));
        }

        /// <summary>
        /// Добавляет или обновляет сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        public void AddOrUpdateUser(TUser user)
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
                        SetAdditionalUserCache(_usersByLogin, GetUserLoginKey(userLogin.LoginProvider, userLogin.ProviderKey), user);
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


        //IUserCacheObserver


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

        private static string GetUserLoginKey(string loginProvider, string providerKey)
        {
            return $"{loginProvider},{providerKey}";
        }

        private TUser GetUserCache(string userId)
        {
            _cacheLockSlim.EnterReadLock();

            try
            {
                return (TUser)_usersById.Get(userId);
            }
            finally
            {
                _cacheLockSlim.ExitReadLock();
            }
        }

        private void SetUserCache(string userId, TUser user)
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

            var removedUser = (TUser)value;

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
                        RemoveAdditionalUserCache(_usersByLogin, GetUserLoginKey(userLogin.LoginProvider, userLogin.ProviderKey));
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

        private TUser GetAdditionalUserCache(IDictionary<string, TUser> additionalCache, string userKey)
        {
            _cacheLockSlim.EnterReadLock();

            try
            {
                TUser user;
                additionalCache.TryGetValue(userKey, out user);
                return user;
            }
            finally
            {
                _cacheLockSlim.ExitReadLock();
            }
        }

        private static void SetAdditionalUserCache(IDictionary<string, TUser> additionalCache, string userKey, TUser user)
        {
            if (!string.IsNullOrEmpty(userKey))
            {
                additionalCache[userKey] = user;
            }
        }

        private static void RemoveAdditionalUserCache(IDictionary<string, TUser> additionalCache, string userKey)
        {
            if (!string.IsNullOrEmpty(userKey))
            {
                additionalCache.Remove(userKey);
            }
        }
    }
}