using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.UserInterface.ViewBuilders.Scripts
{
    /// <summary>
    ///     Компилятор прикладных скриптов.
    /// </summary>
    internal interface IScriptCompiler
    {
        /// <summary>
        ///     Компилировать скрипт.
        /// </summary>
        /// <param name="scripts">Список метаданных скриптов.</param>
        IEnumerable<Script> Compile(IEnumerable scripts);
    }
}