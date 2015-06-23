using System;
using System.Linq;
using System.Linq.Expressions;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Messaging;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;

namespace InfinniPlatform.UserInterface.ViewBuilders.Views
{
    internal static class ViewExtensions
    {
        /// <summary>
        ///     Оповещает шину сообщений асинхронно, когда у источника происходит указанное событие.
        /// </summary>
        public static void NotifyWhenEventAsync(this View source, Expression<Func<View, ScriptDelegate>> sourceEvent,
            Func<dynamic, bool> initArguments = null)
        {
            var sourceEventName = (sourceEvent != null) ? sourceEvent.Body.ToString().Split('.').LastOrDefault() : null;

            NotifyWhenEvent(true, source, sourceEventName, initArguments, source.GetExchange);
        }

        /// <summary>
        ///     Оповещает шину сообщений асинхронно, когда у источника происходит указанное событие.
        /// </summary>
        public static void NotifyWhenEventAsync<T>(this T source, Expression<Func<T, ScriptDelegate>> sourceEvent,
            Func<dynamic, bool> initArguments = null) where T : IViewChild
        {
            var sourceEventName = (sourceEvent != null) ? sourceEvent.Body.ToString().Split('.').LastOrDefault() : null;

            NotifyWhenEvent(true, source, sourceEventName, initArguments, () => source.GetView().GetExchange());
        }

        private static void NotifyWhenEvent(bool async, object source, string sourceEvent,
            Func<dynamic, bool> initArguments, Func<IMessageExchange> exchangeFunc)
        {
            var sourceType = source.GetType();

            if (string.IsNullOrEmpty(sourceEvent))
            {
                throw new ArgumentNullException("sourceEvent", Resources.ShouldSpecifyEvent);
            }


            var sourceEventProp = sourceType.GetProperty(sourceEvent);

            if (sourceEventProp == null)
            {
                throw new ArgumentNullException("sourceEvent",
                    string.Format(Resources.SourceHasNoEvent, sourceType.Name, sourceEvent));
            }


            var sourceEventDelegate = sourceEventProp.GetValue(source) as ScriptDelegate;

            sourceEventDelegate += (context, arguments) =>
            {
                if (initArguments == null || initArguments(arguments))
                {
                    var exchange = exchangeFunc();

                    if (async)
                    {
                        exchange.SendAsync(sourceEvent, arguments);
                    }
                    else
                    {
                        exchange.Send(sourceEvent, arguments);
                    }
                }
            };

            sourceEventProp.SetValue(source, sourceEventDelegate);
        }

        /// <summary>
        ///     Подписывает получателя на указанное сообщение от шины сообщений.
        /// </summary>
        public static void SubscribeOnEvent(this View receiver, Action<View, dynamic> eventHandler,
            Func<dynamic, bool> filterByArguments = null)
        {
            var eventName = (eventHandler != null) ? eventHandler.Method.Name : null;

            SubscribeOnEvent(receiver, eventName, eventHandler, filterByArguments, receiver.GetExchange);
        }

        /// <summary>
        ///     Подписывает получателя на указанное сообщение от шины сообщений.
        /// </summary>
        public static void SubscribeOnEvent<T>(this T receiver, Action<T, dynamic> eventHandler,
            Func<dynamic, bool> filterByArguments = null) where T : IViewChild
        {
            // Warning: Magic!
            var eventName = (eventHandler != null) ? eventHandler.Method.Name : null;

            SubscribeOnEvent(receiver, eventName, eventHandler, filterByArguments,
                () => receiver.GetView().GetExchange());
        }

        /// <summary>
        ///     Подписывает получателя на указанное сообщение от шины сообщений.
        /// </summary>
        public static void SubscribeOnEvent<T>(this T receiver, string eventName, Action<T, dynamic> eventHandler,
            Func<dynamic, bool> filterByArguments = null) where T : IViewChild
        {
            SubscribeOnEvent(receiver, eventName, eventHandler, filterByArguments,
                () => receiver.GetView().GetExchange());
        }

        private static void SubscribeOnEvent<T>(T receiver, string eventName, Action<T, dynamic> eventHandler,
            Func<dynamic, bool> filterByArguments, Func<IMessageExchange> exchangeFunc)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName", Resources.ShouldSpecifyEvent);
            }

            if (eventHandler == null)
            {
                throw new ArgumentNullException("eventHandler", Resources.ShouldSpecifyEventHandler);
            }


            var exchange = exchangeFunc();

            exchange.Subscribe(eventName, arguments =>
            {
                if (filterByArguments == null || filterByArguments(arguments))
                {
                    eventHandler(receiver, arguments);
                }
            });
        }

        public static void InvokeScript(this View target, ScriptDelegate script, Action<dynamic> initArguments = null)
        {
            if (script != null)
            {
                dynamic arguments = new DynamicWrapper();
                dynamic context = target.GetContext();

                arguments.Source = target;

                if (initArguments != null)
                {
                    initArguments(arguments);
                }

                script(context, arguments);
            }
        }
    }
}