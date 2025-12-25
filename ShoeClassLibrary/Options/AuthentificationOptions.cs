namespace ShoeClassLibrary.Options
{
    /// <summary>
    ///Свойства JWT-авторизации
    /// </summary>
    public static class AuthSettings
    {
        public static readonly string secretKey = "314159265358979323846";
        public static readonly string issuer = "myapp";
        public static readonly string audience = "myapp-users";
    }
}