using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TicketHub.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var configUsername = Environment.GetEnvironmentVariable("AppUsername") ?? "";
                var configPassword = Environment.GetEnvironmentVariable("AppPassword") ?? "";

                if (string.IsNullOrEmpty(configUsername) || string.IsNullOrEmpty(configPassword))
                {
                    ViewBag.Error = "Configuration error: Missing credentials in App Settings.";
                    return View();
                }

                if (username == configUsername && password == configPassword)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, "Administrator"),
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = "Invalid username or password.";
                return View();
            }
            catch (Exception ex)
            {
                // Log error for debugging
                Console.WriteLine($"Login error: {ex.Message}");
                ViewBag.Error = "An unexpected error occurred.";
                return View();
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogoutConfirmed()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
} 
//final code