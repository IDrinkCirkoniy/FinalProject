namespace ShoeClassLibrary.DTOs
{
    /// <summary>
    /// Запрос на авторизацию в системе
    /// </summary>
    /// <param name="login">Логин.</param>
    /// <param name="password">Пароль.</param>
    public class LoginRequest(string login, string password)
    {
        public string Login { get; set; } = login;
        public string Password { get; set; } = password;
    }
}
