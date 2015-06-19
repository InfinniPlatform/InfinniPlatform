namespace InfinniPlatform.UserInterface.ViewBuilders.Scripts
{
    /// <summary>
    ///     Делегат функции прикладного скрипта.
    /// </summary>
    /// <param name="context">Контекст функции прикладного скрипта.</param>
    /// <param name="arguments">Аргументы функции прикладного скрипта.</param>
    public delegate void ScriptDelegate(dynamic context, dynamic arguments);
}