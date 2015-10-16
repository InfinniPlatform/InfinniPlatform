using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading;

using InfinniPlatform.Api.Security;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.SystemConfig.UserStorage
{
    sealed class ApplicationUserStoreCache
    {
        public ApplicationUserStoreCache()
        {
            _cacheTimeout = TimeSpan.FromMinutes(Math.Max(AppSettings.GetValue("UserCacheTimeout", 30), 1));
            _cacheLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            _usersById = new MemoryCache("UserCache");
            _usersByName = new ConcurrentDictionary<string, ApplicationUser>();
            _usersByEmail = new ConcurrentDictionary<string, ApplicationUser>();
            _usersByPhone = new ConcurrentDictionary<string, ApplicationUser>();
            _usersByLogin = new ConcurrentDictionary<string, ApplicationUser>();
        }


        private readonly TimeSpan _cacheTimeout;
        private readonly ReaderWriterLockSlim _cacheLockSlim;

        private readonly MemoryCache _usersById;
        private readonly ConcurrentDictionary<string, ApplicationUser> _usersByName;
        private readonly ConcurrentDictionary<string, ApplicationUser> _usersByEmail;
        private readonly ConcurrentDictionary<string, ApplicationUser> _usersByPhone;
        private readonly ConcurrentDictionary<string, ApplicationUser> _usersByLogin;


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
        }

        /// <summary>
        /// Удаляет сведения о пользователе системы.
        /// </summary>
        /// <param name="user">Сведения о пользователе системы.</param>
        public void RemoveUser(ApplicationUser user)
        {
            _cacheLockSlim.EnterWriteLock();

            try
            {
                RemoveUserCache(user.Id);
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


        private static string GetUserLoginKey(ApplicationUserLogin userLogin)
        {
            return string.Format("{0},{1}", userLogin.Provider, userLogin.ProviderKey);
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
    }
}