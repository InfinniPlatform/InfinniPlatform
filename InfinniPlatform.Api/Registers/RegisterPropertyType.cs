using System.Collections.Generic;

namespace InfinniPlatform.Api.Registers
{
    public static class RegisterPropertyType
    {
        /// <summary>
        /// Измерение
        /// </summary>
        public const string Dimension = "Dimension";
        
        /// <summary>
        /// Реквизит
        /// </summary>
        public const string Info = "Info";
        
        /// <summary>
        /// Ресурс
        /// </summary>
        public const string Value = "Value";

        public static IEnumerable<string> GetRegisterPropertyTypes()
        {
            return new[]
	        {
	            Dimension, 
                Info, 
                Value
	        };
        } 
    }
}
