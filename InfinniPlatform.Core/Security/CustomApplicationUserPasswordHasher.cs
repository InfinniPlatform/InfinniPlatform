using InfinniPlatform.Api.Security;

namespace InfinniPlatform.Security
{
	public sealed class CustomApplicationUserPasswordHasher : IApplicationUserPasswordHasher
	{
		/// <summary>
		/// Возвращает хэш пароля.
		/// </summary>
		public string HashPassword(string password)
		{
			return StringHasher.HashValue(password);
		}

		/// <summary>
		/// Проверяет, что пароль соответствует хэшу.
		/// </summary>
		public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
		{
			var success = StringHasher.VerifyValue(hashedPassword, providedPassword);
			if (!success)
			{
				success = new DefaultApplicationUserPasswordHasher().VerifyHashedPassword(hashedPassword, providedPassword);
			}
			return success;
		}
	}
}