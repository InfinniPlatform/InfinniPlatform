using System;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.SchemaReaders
{
    public static class SchemaConverter
    {
        /// <summary>
        ///     Преобразовать свойства JSON, хранимые в виде строк в JSON объекты
        /// </summary>
        /// <param name="resultItem">JSON объект</param>
        public static void ConvertStringToJsonProperties(dynamic resultItem)
        {
            if (resultItem != null)
            {
                dynamic schema = resultItem.Schema;
                if (schema != null && schema.StringifiedJson != null && schema.StringifiedJson == true)
                {
                    try
                    {
                        resultItem.Schema = DynamicWrapperExtensions.ToDynamic(schema.JsonString);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format("Can't parse document schema : {0}", e.Message));
                    }
                }
            }
        }
    }
}