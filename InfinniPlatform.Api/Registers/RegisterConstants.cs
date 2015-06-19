namespace InfinniPlatform.Api.Registers
{
    public static class RegisterConstants
    {
        /// <summary>
        ///     Префикс имени служебного документа регистра
        /// </summary>
        public const string RegisterNamePrefix = "Register_";

        /// <summary>
        ///     Префикс имени служебного документа регистра, хранящего промежуточные итоги
        /// </summary>
        public const string RegisterTotalNamePrefix = "RegisterTotals_";

        /// <summary>
        ///     Служебный документ, хранящий общие данные по всем регистрам конфигурации
        ///     (перед символом '_' будет находится имя конфигурации)
        /// </summary>
        public const string RegistersCommonInfo = "_Registers";

        //////////////////////////////////////////
        // Обязательные поля любой записи регистра
        //////////////////////////////////////////

        /// <summary>
        ///     Регистратор
        /// </summary>
        public const string RegistrarProperty = "Registrar";

        /// <summary>
        ///     Тип Регистратора
        /// </summary>
        public const string RegistrarTypeProperty = "RegistrarType";

        /// <summary>
        ///     Дата Документа
        /// </summary>
        public const string DocumentDateProperty = "DocumentDate";

        /// <summary>
        ///     Тип записи регистра
        /// </summary>
        public const string EntryTypeProperty = "EntryType";
    }
}