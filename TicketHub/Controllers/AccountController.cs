using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TicketHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config; 

        public AccountController(IConfiguration config) // Inject IConfiguration to access appsettings.json
        {
            _config = config;
        }

        [AllowAnonymous] // Allow access without authentication
        [HttpGet] // Display the login form
        public IActionResult Login() // Display the login form
        {
            return View();
        }

        [AllowAnonymous] // Allow access without authentication
        [HttpPost] 
        public async Task<IActionResult> Login(string username, string password)  // Handle login form submission
        {
            var configUsername = _config["username"];
            var configPassword = _config["password"];

            if (username == configUsername && password == configPassword) // Validate credentials against appsettings.json
            {
                var claims = new List<Claim> // Create user claims
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Name, "Administrator"),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); // Create claims identity
                var principal = new ClaimsPrincipal(identity); // Create claims principal

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal); // Sign in the user

                string? returnUrl = Request.Query["returnUrl"]; // Redirect to returnUrl if specified
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home"); // Redirect to home page after successful login
            }

            ViewBag.Error = "Invalid username or password."; // Display error message for invalid credentials
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Logout() // Display the logout confirmation page
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogoutConfirmed() // Handle logout confirmation
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Sign out the user
            return RedirectToAction("Login", "Account"); // Redirect to login page after logout
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() // Display access denied page
        {
            return View();
        }
    }
}