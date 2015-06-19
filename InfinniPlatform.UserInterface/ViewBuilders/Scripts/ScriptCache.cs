using System;
using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.UserInterface.ViewBuilders.Scripts
{
    /// <summary>
    ///     Кэш прикладных скриптов.
    /// </summary>
    internal sealed class ScriptCache
    {
        private readonly Dictionary<object, IEnumerable<Script>> _scriptCache =
            new Dictionary<object, IEnumerable<Script>>();

        private readonly IScriptCompiler _scriptCompiler;

        public ScriptCache(IScriptCompiler scriptCompiler)
        {
            if (scriptCompiler == null)
            {
                throw new ArgumentNullException("scriptCompiler");
            }

            _scriptCompiler = scriptCompiler;
        }

        public IEnumerable<Script> GetScripts(object key, IEnumerable scripts)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            IEnumerable<Script> result;

            if (_scriptCache.TryGetValue(key, out result) == false)
            {
                _scriptCache[key] = (result = _scriptCompiler.Compile(scripts));
            }

            return result;
        }
    }
}