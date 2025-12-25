using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoeClassLibrary.DTOs;
using ShoeClassLibrary.Models;
using ShoesClassLibrary.Services;

namespace ShoesWebApp.Pages
{
    public class LoginModel(AuthenticationService service) : PageModel
    {
        private AuthenticationService _authenticationService = service;

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnPostLoginAsync()
        {
            LoginRequest login = new(User.Login, User.Password);

            var user = await _authenticationService.AuthentificationUserAsync(login);
            var role = await _authenticationService.GetUserRoleAsync(User.Login);

            if (user is null)
                return Page();

            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("SecondName", user.SecondName);
            HttpContext.Session.SetString("Patronymic", user.Patronymic);
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Role", role.Name);

            return RedirectToPage("/Shoes/Index");
        }

        public async Task<IActionResult> OnPostGuest()
        {
            HttpContext.Session.Clear();

            HttpContext.Session.SetString("Role", "guest");

            return RedirectToPage("/Shoes/Index");
        }
    }
}
