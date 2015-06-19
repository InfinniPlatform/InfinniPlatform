namespace InfinniPlatform.Api.ContextComponents
{
    /// <summary>
    ///     Компонент верификации пароля пользователя
    /// </summary>
    public interface IPasswordVerifierComponent
    {
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}