using System;
using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент для регистрации пользовательских зависимостей внутри глобального контекста
    /// </summary>
    public sealed class DependencyContainerComponent : IDependencyContainerComponent
    {
        private readonly Action<object> _registerInstanceFunc;
        private readonly Action<Type> _registerTypeFunc;
        private readonly Func<Type, object> _resolveDependencyFunc;
        private readonly Action _updateDependencyAction;

        public DependencyContainerComponent(Action<Type> registerTypeFunc,
            Action<object> registerInstanceFunc, Func<Type, object> resolveDependencyFunc, Action updateDependencyAction)
        {
            _registerTypeFunc = registerTypeFunc;
            _registerInstanceFunc = registerInstanceFunc;
            _resolveDependencyFunc = resolveDependencyFunc;
            _updateDependencyAction = updateDependencyAction;
        }

        public void RegisterDependencyType<TImplementation>()
        {
            _registerTypeFunc.Invoke(typeof (TImplementation));
        }

        public void RegisterDependencyInstance<TImplementation>(TImplementation instance)
        {
            _registerInstanceFunc(instance);
        }

        public T ResolveDependency<T>()
        {
            return (T) _resolveDependencyFunc(typeof (T));
        }

        public void UpdateDependencies()
        {
            _updateDependencyAction();
        }
    }
}