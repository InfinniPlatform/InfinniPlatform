using System;
using System.Reflection;

namespace InfinniPlatform.Core.Abstractions.IoC
{
    /// <summary>
    /// Универсальный обработчик разрешения зависимостей, передаваемых через параметры конструкторов.
    /// </summary>
    public class SimpleContainerParameterResolver : IContainerParameterResolver
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="canResolve">Функция для определения, какие параметры конструктора могут быть разрешены с помощью данного обработчика.</param>
        /// <param name="resolve">Функция разрешения значения для указанного параметра конструктора.</param>
        public SimpleContainerParameterResolver(Func<ParameterInfo, IContainerResolver, bool> canResolve, Func<ParameterInfo, IContainerResolver, object> resolve)
        {
            _canResolve = canResolve;
            _resolve = resolve;
        }


        private readonly Func<ParameterInfo, IContainerResolver, bool> _canResolve;
        private readonly Func<ParameterInfo, IContainerResolver, object> _resolve;


        public bool CanResolve(ParameterInfo parameterInfo, IContainerResolver resolver)
        {
            return _canResolve(parameterInfo, resolver);
        }

        public object Resolve(ParameterInfo parameterInfo, IContainerResolver resolver)
        {
            return _resolve(parameterInfo, resolver);
        }
    }
}