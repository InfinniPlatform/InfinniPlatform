using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;

using Microsoft.Owin;
using Microsoft.Owin.Helpers;

namespace InfinniPlatform.Owin.Formatting
{
    /// <summary>
    /// Предоставляет методы чтения и записи тела запроса и ответа, представленных в формате
    /// "application/x-www-form-urlencoded".
    /// </summary>
    public sealed class FormBodyFormatter : IBodyFormatter
    {
        public static readonly FormBodyFormatter Instance = new FormBodyFormatter();

        public string ContentType => "application/x-www-form-urlencoded";

        public object ReadBody(IOwinRequest request)
        {
            string formText;

            // Чтение текста запроса
            using (var reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                formText = reader.ReadToEnd();
            }

            // Разбор текста запроса
            var formCollection = WebHelpers.ParseForm(formText);

            // Преобразование в объект
            var result = new DynamicWrapper();

            foreach (var item in formCollection)
            {
                if (item.Value != null && item.Value.Length == 1)
                {
                    result[item.Key] = item.Value[0];
                }
                else
                {
                    result[item.Key] = item.Value;
                }
            }

            return result;
        }

        public Task WriteBody(IOwinResponse response, object value)
        {
            throw new NotImplementedException();
        }
    }
}