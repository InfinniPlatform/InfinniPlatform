using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.ContextComponents
{
	/// <summary>
	///   Компонент верификации пароля пользователя
	/// </summary>
	public interface IPasswordVerifierComponent
	{
		bool VerifyPassword(string hashedPassword, string providedPassword);
	}
}
