using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Security;

namespace InfinniPlatform.Security
{
	public sealed class CustomApplicationUserPasswordHasher : IApplicationUserPasswordHasher
	{
		private readonly IGlobalContext _globalContext;

		public CustomApplicationUserPasswordHasher(IGlobalContext globalContext)
		{
			_globalContext = globalContext;
		}

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
				success = _globalContext.GetComponent<IPasswordVerifierComponent>().VerifyPassword(hashedPassword,providedPassword);
			}
			return success;
		}
	}
}
