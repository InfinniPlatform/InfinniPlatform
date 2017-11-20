using System;
using System.Collections.Generic;
using System.Security.Principal;

using InfinniPlatform.Auth.HttpService.Properties;
using InfinniPlatform.Session;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Auth.HttpService
{
    /// <summary>
    /// Предоставляет методы для вызова зарегистрированных обработчиков событий пользователя.
    /// </summary>
    public class UserEventHandlerInvoker
    {
        public UserEventHandlerInvoker(IEnumerable<IUserEventHandler> userEventHandlers, ILogger<UserEventHandlerInvoker> logger)
        {
            _userEventHandlers = userEventHandlers;
            _logger = logger;
        }


        private readonly IEnumerable<IUserEventHandler> _userEventHandlers;
        private readonly ILogger _logger;


        /// <summary>
        /// Вызывается после входа пользователя в систему.
        /// </summary>
        /// <param name="identity">Идентификационные данные пользователя.</param>
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
                        // Исключения игнорируются, так как они не должны нарушить работоспособность основного механизма

                        _logger.LogError(string.Format(Resources.HandlingUserEventCompletedWithException, nameof(userEventHandler.OnAfterSignIn)), exception);
                    }
                }
            }
        }


        /// <summary>
        /// Вызывается перед выходом пользователя из системы.
        /// </summary>
        /// <param name="identity">Идентификационные данные пользователя.</param>
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
                        // Исключения игнорируются, так как они не должны нарушить работоспособность основного механизма

                        _logger.LogError(string.Format(Resources.HandlingUserEventCompletedWithException, nameof(userEventHandler.OnBeforeSignOut)), exception);
                    }
                }
            }
        }
    }
}