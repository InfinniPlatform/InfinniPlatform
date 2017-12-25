using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Auth.UserCache
{
    /// <summary>
    /// Application user cache.
    /// </summary>
    /// <typeparam name="TUser">User type.</typeparam>
    public class UserCache<TUser> : IUserCacheObserver where TUser : AppUser
    {
        private readonly AppOptions _appOptions;
        private readonly IBroadcastProducer _broadcastProducer;

        private readonly ReaderWriterLockSlim _cacheLockSlim;
        private readonly TimeSpan _cacheTimeout;
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<string, TUser> _usersByEmail;

        private readonly MemoryCache _usersById;
        private readonly ConcurrentDictionary<string, TUser> _usersByLogin;
        private readonly ConcurrentDictionary<string, TUser> _usersByName;
        private readonly ConcurrentDictionary<string, TUser> _usersByPhone;

        /// <summary>
        /// Initializes a new instance of <see cref="UserCache{TUser}" />.
        /// </summary>
        /// <param name="options">Authentication options.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="broadcastProducer">Broadcast messages producer.</param>
        /// <param name="appOptions">Application options.</param>
        public UserCache(AuthOptions options,
                         ILogger<UserCache<TUser>> logger,
                         IBroadcastProducer broadcastProducer,
                         AppOptions appOptions)
        {
            var cacheTimeout = options.UserCacheTimeout <= 0
                                   ? AuthOptions.DefaultUserCacheTimeout
                                   : options.UserCacheTimeout;

            _cacheTimeout = TimeSpan.FromMinutes(cacheTimeout);
            _logger = logger;
            _broadcastProducer = broadcastProducer;
            _appOptions = appOptions;

            _cacheLockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            _usersById = new MemoryCache(new MemoryCacheOptions());
            _usersByName = new ConcurrentDictionary<string, TUser>();
            _usersByEmail = new ConcurrentDictionary<string, TUser>();
            _usersByPhone = new ConcurrentDictionary<string, TUser>();
            _usersByLogin = new ConcurrentDictionary<string, TUser>();
        }


        //IUserCacheObserver


        /// <inheritdoc />
        public Task ProcessMessage(Message<string> message)
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
                _logger.LogError(exception);
            }

            return Task.CompletedTask;
        }


        /// <summary>
        /// Returns user information by identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        public TUser FindUserById(string userId)
        {
            return GetUserCache(userId);
        }

        /// <summary>
        /// Returns user information by normalized name.
        /// </summary>
        /// <param name="normalizedUserName">Normalized user name.</param>
        public TUser FindUserByUserName(string normalizedUserName)
        {
            return GetAdditionalUserCache(_usersByName, normalizedUserName);
        }

        /// <summary>
        /// Returns user information by email.
        /// </summary>
        /// <param name="email">User email.</param>
        public TUser FindUserByEmail(string email)
        {
            return GetAdditionalUserCache(_usersByEmail, email);
        }

        /// <summary>
        /// Returns user information by phone number.
        /// </summary>
        /// <param name="phoneNumber">User phone number.</param>
        public TUser FindUserByPhoneNumber(string phoneNumber)
        {
            return GetAdditionalUserCache(_usersByPhone, phoneNumber);
        }

        /// <summary>
        /// Returns user information by external login.
        /// </summary>
        /// <param name="loginProvider">External login provider.</param>
        /// <param name="providerKey">External login provider key.</param>
        public TUser FindUserByLogin(string loginProvider, string providerKey)
        {
            return GetAdditionalUserCache(_usersByLogin, GetUserLoginKey(loginProvider, providerKey));
        }

        /// <summary>
        /// Adds or updates user information in cache.
        /// </summary>
        /// <param name="user">User information.</param>
        public void AddOrUpdateUser(TUser user)
        {
            _cacheLockSlim.EnterWriteLock();

            try
            {
                SetUserCache(user._id.ToString(), user);
                SetAdditionalUserCache(_usersByName, user.NormalizedUserName, user);
                SetAdditionalUserCache(_usersByEmail, user.NormalizedEmail, user);
                SetAdditionalUserCache(_usersByPhone, user.PhoneNumber, user);

                var userLogins = user.Logins;

                if (userLogins != null)
                {
                    foreach (var userLogin in userLogins)
                    {
                        SetAdditionalUserCache(_usersByLogin, GetUserLoginKey(userLogin.LoginProvider, userLogin.ProviderKey), user);
                    }
                }

                NotifyOnUserChanged(user._id.ToString());
            }
            finally
            {
                _cacheLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes user information in cache.
        /// </summary>
        /// <param name="userId">User identifier.</param>
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
                return (TUser) _usersById.Get(userId);
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

            var removedUser = (TUser) value;

            _cacheLockSlim.EnterWriteLock();

            try
            {
                RemoveAdditionalUserCache(_usersByName, removedUser.NormalizedUserName);
                RemoveAdditionalUserCache(_usersByEmail, removedUser.NormalizedEmail);
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