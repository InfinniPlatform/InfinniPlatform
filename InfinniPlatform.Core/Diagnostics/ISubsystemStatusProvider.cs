﻿using System.Threading.Tasks;

namespace InfinniPlatform.Core.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы.
    /// </summary>
    public interface ISubsystemStatusProvider
    {
        /// <summary>
        /// Имя подсистемы.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Состояние подсистемы.
        /// </summary>
        Task<object> GetStatus();
    }
}