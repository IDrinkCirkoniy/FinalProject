using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoesClassLibrary.Services;

namespace ShoesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController(AuthenticationService AuthService) : ControllerBase
    {
        private readonly AuthenticationService _authService = AuthService; // Объект для работы с авторизацией

        [AllowAnonymous] // Атрибут дающий доступ неавторизованным пользователям
        [HttpPost("login")] // Метод Post с конечной точкой Login
        public ActionResult Post(string login, string password)
        {
            var token = _authService.LoginUser(login, password);  // Вызов сервиса аутентификации с получением токена

            return token is null
                ? Unauthorized()  // 401 Неавторизирован
                : Ok(new { token });  // 200 Авторизирован вернули токен
        }
    }
}
