using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;
using System.Net.Http.Json;
namespace RaceDay.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public AccountController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using var response = await _apiService.PostAsync(
                "api/auth/register", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Registration failed. Check your details or email.");
                return View(model);
            }

            TempData["Message"] =
                "Registration successful! Please log in.";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using var response = await _apiService.PostAsync(
                "api/auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Invalid email or password.");
                return View(model);
            }

            var profile = await response.Content.ReadFromJsonAsync<UserProfile>();

            if (profile == null)
            {
                ModelState.AddModelError("",
                    "Could not retrieve your account details.");
                return View(model);
            }

            HttpContext.Session.SetString("Role", profile.Role);
            HttpContext.Session.SetString("Name", profile.FullName);

            return RedirectToAction("Index", "Events");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            using var response = await _apiService.PostAsync(
                "api/auth/logout", new { });

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Events");
        }
    }
}
