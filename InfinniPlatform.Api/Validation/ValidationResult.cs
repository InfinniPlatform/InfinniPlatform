using System.Collections.Generic;

namespace InfinniPlatform.Api.Validation
{
	/// <summary>
	/// Результат проверки объекта.
	/// </summary>
	public sealed class ValidationResult
	{
		public ValidationResult()
		{
			IsValid = true;
			Items = new List<dynamic>();
		}

        public ValidationResult(bool isValid)
        {
            IsValid = isValid;
            Items = new List<dynamic>();
        }

		/// <summary>
		/// Возвращает или устанавливает признак успешности проверки.
		/// </summary>
		public bool IsValid { get; set; }

		/// <summary>
		/// Возвращает или устанавливает список результатов проверки свойств объекта.
		/// </summary>
		public List<dynamic> Items { get; set; }
	}
}