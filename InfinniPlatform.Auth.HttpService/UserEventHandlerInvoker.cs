using System;
using System.Collections.Generic;
using System.Security.Principal;

using InfinniPlatform.Auth.HttpService.Properties;
using InfinniPlatform.Session;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// Provider methods for handling user authentication events.
    /// </summary>
    public class UserEventHandlerInvoker
    {
        /// <summary>
        /// Initializes a new instance of <see cref="UserEventHandlerInvoker" />.
        /// </summary>
        /// <param name="userEventHandlers">User authentication events handler.</param>
        /// <param name="logger">Logger.</param>
        public UserEventHandlerInvoker(IEnumerable<IUserEventHandler> userEventHandlers,
                                       ILogger<UserEventHandlerInvoker> logger)
        {
            _userEventHandlers = userEventHandlers;
            _logger = logger;
        }


        private readonly IEnumerable<IUserEventHandler> _userEventHandlers;
        private readonly ILogger _logger;


        /// <summary>
        /// Invokes after user sign in.
        /// </summary>
        /// <param name="identity">User identity.</param>
        public async void OnAfterSignIn(IIdentity identity)
        {
            if (identity != null)
            {
                foreach (var userEventHandler in _userEventHandlers)
                {
                    try
                    {
                        await userEventHandler.OnAfterSignIn(identity);
                    }
                    catch (Exception exception)
                    {
                        // Exceptions from handlers are ignored, so they won't interfere with auth process.

                        _logger.LogError(string.Format(Resources.HandlingUserEventCompletedWithException, nameof(userEventHandler.OnAfterSignIn)), exception);
                    }
                }
            }
        }


        /// <summary>
        /// Invokes before user sign out.
        /// </summary>
        /// <param name="identity">User identity.</param>
        public async void OnBeforeSignOut(IIdentity identity)
        {
            if (identity != null)
            {
                foreach (var userEventHandler in _userEventHandlers)
                {
                    try
                    {
                        await userEventHandler.OnBeforeSignOut(identity);
                    }
                    catch (Exception exception)
                    {
                        // Exceptions from handlers are ignored, so they won't interfere with auth process.

                        _logger.LogError(string.Format(Resources.HandlingUserEventCompletedWithException, nameof(userEventHandler.OnBeforeSignOut)), exception);
                    }
                }
            }
        }
    }
}