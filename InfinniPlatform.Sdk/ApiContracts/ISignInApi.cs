using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.ApiContracts
{
    public interface ISignInApi
    {
        /// <summary>
        ///   Зарегистрироваться с использованием внутреннего хранилища пользователей
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="remember">Запомнить пользователя</param>
        /// <returns>Результат попытки регистрации</returns>
        dynamic SignInInternal(string userName, string password, bool remember);

        /// <summary>
        ///   Сменить пароль пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="oldPassword">Старый пароль пользователя</param>
        /// <param name="newPassword">Новый пароль пользователя</param>
        /// <returns>Признак успешной смены пароля пользователя</returns>
        dynamic ChangePassword(string userName, string oldPassword, string newPassword);

        /// <summary>
        ///   Выйти из системы
        /// </summary>
        /// <returns>Признак успешного выхода из системы</returns>
        dynamic SignOut();
    }
}
