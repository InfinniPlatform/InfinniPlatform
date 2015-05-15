using System;

using InfinniPlatform.Api.Security;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.Modules
{
	/// <summary>
	/// Предоставляет методы хэширования пароля.
	/// </summary>
	sealed class IdentityApplicationUserPasswordHasher : IPasswordHasher
	{
		public IdentityApplicationUserPasswordHasher(IApplicationUserPasswordHasher passwordHasher)
		{
			if (passwordHasher == null)
			{
				throw new ArgumentNullException("passwordHasher");
			}

			_passwordHasher = passwordHasher;
		}


		private readonly IApplicationUserPasswordHasher _passwordHasher;


		public string HashPassword(string password)
		{
			return _passwordHasher.HashPassword(password);
		}

		public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
		{
			return _passwordHasher.VerifyHashedPassword(hashedPassword, providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
		}
	}
}