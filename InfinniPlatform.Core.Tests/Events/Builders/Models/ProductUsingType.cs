namespace InfinniPlatform.Core.Tests.Events.Builders.Models
{
	/// <summary>
	///   Тип применения
	/// </summary>
	public enum ProductUsingType
	{
		Unknown, // – нет данных  
		NotAllowed, // – запрещен к применению 
		UseWithCare, // – применять с осторожностью
		CanUse // – разрешен к применению.
	}
}