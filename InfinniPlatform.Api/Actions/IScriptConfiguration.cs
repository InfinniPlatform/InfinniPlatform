using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.Api.Actions
{
	/// <summary>
	///   Конфигурация метаданных прикладных скриптов
	/// </summary>
	public interface IScriptConfiguration
	{
		/// <summary>
		///   Зарегистрировать встроенный в проект скриптовый модуль
		/// </summary>
		/// <param name="unitIdentifier">Идентификатор метаданных</param>
		/// <param name="actionUnitBuilder">Конструктор модуля</param>
		void RegisterActionUnitEmbedded(string unitIdentifier, IActionOperatorBuilder actionUnitBuilder);

	    /// <summary>
	    ///   Зарегистрировать скриптовый модуль из распределенного хранилища
	    /// </summary>
	    /// <param name="unitIdentifier">Идентификатор метаданных</param>
	    /// <param name="type">Тип модуля</param>
	    /// <param name="version"></param>
	    void RegisterActionUnitDistributedStorage(string unitIdentifier, string type, string version = null);

		/// <summary>
		///   Получить модуль по идентификатору 
		/// </summary>
		/// <param name="unitIdentifier">Идентификатор модуля</param>
		/// <returns>Скриптовый оператор</returns>
		IActionOperator GetAction(string unitIdentifier);

		/// <summary>
		///  Инииализировать хранилище прикладных модулей
		/// </summary>
		/// <param name="version"></param>
		void InitActionUnitStorage(string version);

	    /// <summary>
        ///   Идентификатор модуля/конфигурации, к которой относится конфигурация прикладных скриптов
        /// </summary>
	    string ModuleName { get; set; }

		/// <summary>
	    ///  Зарегистрировать модуль валидации
	    /// </summary>
	    /// <param name="unitIdentifier">Идентификатор метаданных валидатора</param>
	    /// <param name="validationUnitBuilder">Конструктор валидации</param>
	    void RegisterValidationUnitEmbedded(string unitIdentifier, IValidationUnitBuilder validationUnitBuilder);

	    /// <summary>
	    /// Зарегистрировать модули валидации
	    /// </summary>
	    /// <param name="unitIdentifier"></param>
	    /// <param name="type"></param>
	    /// <param name="version"></param>
	    void RegisterValidationUnitDistributedStorage(string unitIdentifier, string type, string version = null);

	    /// <summary>
	    ///   Получить оператор валидации по указанному идентификатору
	    /// </summary>
	    /// <param name="unitIdentifier">Идентификатор валидации</param>
	    /// <returns>Оператор валидации</returns>
	    IValidationOperator GetValidator(string unitIdentifier);

	    /// <summary>
	    ///   Получить исполнитель скриптов конфигурации
	    /// </summary>
	    /// <returns></returns>
	    IScriptProcessor GetScriptProcessor(string version);
	}
}
