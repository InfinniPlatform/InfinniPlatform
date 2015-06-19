using System.Collections.Generic;

namespace InfinniPlatform.Api.Registers
{
    public static class RegisterType
    {
        /// <summary>
        ///     Регистр сведений
        /// </summary>
        public const string Info = "Info";

        /// <summary>
        ///     Регистр остатков
        /// </summary>
        public const string Balance = "Balance";

        /// <summary>
        ///     Регистр оборотов
        /// </summary>
        public const string Turnover = "Turnover";

        public static IEnumerable<string> GetRegisterTypes()
        {
            return new[]
            {
                Info,
                Balance,
                Turnover
            };
        }
    }
}