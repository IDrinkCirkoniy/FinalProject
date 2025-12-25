using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoeClassLibrary.Contexts;
using ShoeClassLibrary.DTOs;
using ShoeClassLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoesClassLibrary.Services
{
    /// <summary>
    /// Сервис для аутентификации пользователей
    /// </summary>
    public class AuthenticationService(ShoeContext context)
    {
        private readonly ShoeContext _context = context;
        private int _jwtActiveMinutes = 10;
        public static readonly string secretKey = "314159265358979323846";

        /// <summary>
        /// Создает зашифрованный JWT-токен
        /// </summary>
        /// <param name="user">Объект пользователя с ролью</param>
        /// <returns>Строка токена в формате JWT</returns>
        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var authority = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Наполнение токена информацией о пользователе
            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new(ClaimTypes.Name, user.Login),
                new(ClaimTypes.Role, user.Role.Name)
            };

            var token = new JwtSecurityToken(
                signingCredentials: authority,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10) // Время жизни токена
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private async Task<User> GetUserByLoginAsync(string login)
            => await _context.Users
                .FirstOrDefaultAsync(u => u.Login == login) ?? null!;

        /// <summary>
        /// Безопасная проверка пароля с использованием хеширования BCrypt
        /// </summary>
        private bool VerifyPassword(string password, string passwordHash)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);

        /// <summary>
        /// Поиск пользователя в базе данных по логину
        /// </summary>

        /// <summary>
        /// Проверяет учетные данные пользователя при входе
        /// </summary>
        /// <param name="request">Запрос на вход с логином и паролем</param>
        /// <returns>Объект пользователя в случае успеха, иначе null</returns>
        public async Task<User?> AuthentificationUserAsync(LoginRequest request)
        {
            string login = request.Login;
            string password = request.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;

            var user = await GetUserByLoginAsync(login);
            if (user is null)
                return null;

            // Сравнение введенного пароля с хешем из БД
            return VerifyPassword(password, user.Password) ? user : null;
        }
            public async Task<Role?> GetUserRoleAsync(string login)
        {
            var user = await _context.Users
                .Include(c => c.Role) //дополнительно загружает Роль
                .FirstOrDefaultAsync(cu => cu.Login == login); // Ассинхронный метод выборки первого объекта, где совпал логин

            return user is not null ?
                 user.Role :
                 null; // если пользователь существует, возвращает роль
        }
    

        /// <summary>
        /// Выполняет вход пользователя и возвращает токен
        /// </summary>
        public string LoginUser(string login, string password)
        {
            var user = _context.Users
                .Include(u => u.Role) // Загрузка роли
                .FirstOrDefault(u => u.Login == login && u.Password == password); // Метод выборки первого объекта с совпавшим логином и паролем

            if (user == null)
                return null!;

            return GenerateToken(user);
        }
    }
}
