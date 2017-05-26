using System;
using System.Collections.Generic;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.IoC;

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Application layers mapping to keep ordered registration of middlewares.
    /// </summary>
    public class AppLayersMap
    {
        private readonly Dictionary<Type, List<IAppLayer>> _orderedLayers = new Dictionary<Type, List<IAppLayer>>
        {
            {typeof(IGlobalHandlingAppLayer), new List<IAppLayer>()},
            {typeof(IErrorHandlingAppLayer), new List<IAppLayer>()},
            {typeof(IBeforeAuthenticationAppLayer), new List<IAppLayer>()},
            {typeof(IAuthenticationBarrierAppLayer), new List<IAppLayer>()},
            {typeof(IExternalAuthenticationAppLayer), new List<IAppLayer>()},
            {typeof(IInternalAuthenticationAppLayer), new List<IAppLayer>()},
            {typeof(IAfterAuthenticationAppLayer), new List<IAppLayer>()},
            {typeof(IBusinessAppLayer), new List<IAppLayer>()}
        };

        private readonly IContainerResolver _resolver;

        public AppLayersMap(IContainerResolver resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Returns application layers map.
        /// </summary>
        public Dictionary<Type, List<IAppLayer>> GetMap()
        {
            return _orderedLayers;
        }

        /// <summary>
        /// Adds user defined <see cref="IGlobalHandlingAppLayer"/>.
        /// </summary>
        /// <typeparam name="T">Application layer instance type.</typeparam>
        public void AddGlobalHandlingAppLayer<T>() where T : class, IAppLayer
        {
            AddToList<T, IGlobalHandlingAppLayer>();
        }

        /// <summary>
        /// Adds user defined <see cref="IGlobalHandlingAppLayer"/>.
        /// </summary>
        /// <param name="appLayerType">Application layer instance type.</param>
        public void AddGlobalHandlingAppLayer(Type appLayerType)
        {
            AddToList(appLayerType, typeof(IGlobalHandlingAppLayer));
        }

        /// <summary>
        /// Adds user defined <see cref="IErrorHandlingAppLayer"/>.
        /// </summary>
        /// <typeparam name="T">Application layer instance type.</typeparam>
        public void AddErrorHandlingAppLayer<T>() where T : class, IAppLayer
        {
            AddToList<T, IErrorHandlingAppLayer>();
        }

        /// <summary>
        /// Adds user defined <see cref="IErrorHandlingAppLayer"/>.
        /// </summary>
        /// <param name="appLayerType">Application layer instance type.</param>
        public void AddErrorHandlingAppLayer(Type appLayerType)
        {
            AddToList(appLayerType, typeof(IErrorHandlingAppLayer));
        }

        /// <summary>
        /// Adds user defined <see cref="IBeforeAuthenticationAppLayer"/>.
        /// </summary>
        /// <typeparam name="T">Application layer instance type.</typeparam>
        public void AddBeforeAuthenticationAppLayer<T>() where T : class, IAppLayer
        {
            AddToList<T, IBeforeAuthenticationAppLayer>();
        }

        /// <summary>
        /// Adds user defined <see cref="IBeforeAuthenticationAppLayer"/>.
        /// </summary>
        /// <param name="appLayerType">Application layer instance type.</param>
        public void AddBeforeAuthenticationAppLayer(Type appLayerType)
        {
            AddToList(appLayerType, typeof(IBeforeAuthenticationAppLayer));
        }

        /// <summary>
        /// Adds user defined <see cref="IAuthenticationBarrierAppLayer"/>.
        /// </summary>
        /// <typeparam name="T">Application layer instance type.</typeparam>
        public void AddAuthenticationBarrierAppLayer<T>() where T : class, IAppLayer
        {
            AddToList<T, IAuthenticationBarrierAppLayer>();
        }

        /// <summary>
        /// Adds user defined <see cref="IAuthenticationBarrierAppLayer"/>.
        /// </summary>
        /// <param name="appLayerType">Application layer instance type.</param>
        public void AddAuthenticationBarrierAppLayer(Type appLayerType)
        {
            AddToList(appLayerType, typeof(IAuthenticationBarrierAppLayer));
        }

        /// <summary>
        /// Adds user defined <see cref="IExternalAuthenticationAppLayer"/>.
        /// </summary>
        /// <typeparam name="T">Application layer instance type.</typeparam>
        public void AddExternalAuthenticationAppLayer<T>() where T : class, IAppLayer
        {
            AddToList<T, IExternalAuthenticationAppLayer>();
        }

        /// <summary>
        /// Adds user defined <see cref="IExternalAuthenticationAppLayer"/>.
        /// </summary>
        /// <param name="appLayerType">Application layer instance type.</param>
        public void AddExternalAuthenticationAppLayer(Type appLayerType)
        {
            AddToList(appLayerType, typeof(IExternalAuthenticationAppLayer));
        }

        /// <summary>
        /// Adds user defined <see cref="IInternalAuthenticationAppLayer"/>.
        /// </summary>
        /// <typeparam name="T">Application layer instance type.</typeparam>
        public void AddInternalAuthenticationAppLayer<T>() where T : class, IAppLayer
        {
            AddToList<T, IInternalAuthenticationAppLayer>();
        }

        /// <summary>
        /// Adds user defined <see cref="IInternalAuthenticationAppLayer"/>.
        /// </summary>
        /// <param name="appLayerType">Application layer instance type.</param>
        public void AddInternalAuthenticationAppLayer(Type appLayerType)
        {
            AddToList(appLayerType, typeof(IInternalAuthenticationAppLayer));
        }

        /// <summary>
        /// Adds user defined <see cref="IAfterAuthenticationAppLayer"/>.
        /// </summary>
        /// <typeparam name="T">Application layer instance type.</typeparam>
        public void AddAfterAuthenticationAppLayer<T>() where T : class, IAppLayer
        {
            AddToList<T, IAfterAuthenticationAppLayer>();
        }

        /// <summary>
        /// Adds user defined <see cref="IAfterAuthenticationAppLayer"/>.
        /// </summary>
        /// <param name="appLayerType">Application layer instance type.</param>
        public void AddAfterAuthenticationAppLayer(Type appLayerType)
        {
            AddToList(appLayerType, typeof(IAfterAuthenticationAppLayer));
        }

        /// <summary>
        /// Adds user defined <see cref="IBusinessAppLayer"/>.
        /// </summary>
        /// <typeparam name="T">Application layer instance type.</typeparam>
        public void AddBusinessAppLayer<T>() where T : class, IAppLayer
        {
            AddToList<T, IBusinessAppLayer>();
        }

        /// <summary>
        /// Adds user defined <see cref="IBusinessAppLayer"/>.
        /// </summary>
        /// <param name="appLayerType">Application layer instance type.</param>
        public void AddBusinessAppLayer(Type appLayerType)
        {
            AddToList(appLayerType, typeof(IBusinessAppLayer));
        }

        private void AddToList<TLayer, TLayerType>() where TLayer : class, IAppLayer
                                                     where TLayerType : IAppLayer
        {
            var appLayer = _resolver.Resolve<TLayer>();
            _orderedLayers[typeof(TLayerType)].Add(appLayer);
        }

        private void AddToList(Type type, Type appLayerType)
        {
            var appLayer = _resolver.Resolve(type) as IAppLayer;
            _orderedLayers[appLayerType].Add(appLayer);
        }
    }
}