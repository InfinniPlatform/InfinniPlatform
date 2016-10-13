﻿using System.Collections;

namespace InfinniPlatform.Agent.InfinniNode
{
    /// <summary>
    /// Предоставляет доступ к переменным окружения.
    /// </summary>
    public interface IEnvironmentVariableProvider
    {
        /// <summary>
        /// Возвращает список переменных окружения.
        /// </summary>
        IDictionary GetAll();

        /// <summary>
        /// Возвращает переменную окружения по имени.
        /// </summary>
        /// <param name="name">Имя переменной окружения.</param>
        IDictionary Get(string name);
    }
}