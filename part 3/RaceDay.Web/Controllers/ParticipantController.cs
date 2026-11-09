
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public ParticipantController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        // Participant Dashboard
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            ViewBag.ParticipantName =
                HttpContext.Session.GetString("Name");

            return View();
        }

        // Display enrolment form
        [HttpGet]
        public async Task<IActionResult> Enrol(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var categories = await _apiService.GetAsync<List<Category>>(
                $"api/categories?eventId={id}");

            var model = new EnrolmentViewModel
            {
                RaceEventId = id,
                Categories = categories ?? new List<Category>()
            };

            return View(model);
        }

        // Submit enrolment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(EnrolmentViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                model.Categories =
                    await _apiService.GetAsync<List<Category>>(
                        $"api/categories?eventId={model.RaceEventId}")
                    ?? new List<Category>();

                return View(model);
            }

            var request = new EnrolmentRequest
            {
                RaceEventId = model.RaceEventId,
                CategoryId = model.CategoryId
            };

            using var response = await _apiService.PostAsync(
                "api/enrolments", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Enrolment failed. Please check your selected category.");

                model.Categories =
                    await _apiService.GetAsync<List<Category>>(
                        $"api/categories?eventId={model.RaceEventId}")
                    ?? new List<Category>();

                return View(model);
            }

            TempData["Message"] = "You have successfully enrolled!";

            return RedirectToAction("Dashboard");
        }

        // View participant enrolments
        public async Task<IActionResult> MyEnrolments()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var enrolments =
                await _apiService.GetAsync<List<ParticipantEnrolment>>(
                    "api/enrolments/mine");

            return View(enrolments ?? new List<ParticipantEnrolment>());
        }

        // View participant results
        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var results = await _apiService.GetAsync<List<Result>>(
                "api/results/mine");

            return View(results ?? new List<Result>());
        }

        // View participant profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var profile = await _apiService.GetAsync<ProfileViewModel>(
                "api/users/me");

            if (profile == null)
                return RedirectToAction("Dashboard");

            return View(profile);
        }

        // Update participant profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            using var response = await _apiService.PutAsync(
                "api/users/me", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to update your profile.");

                return View(model);
            }

            TempData["Message"] = "Profile updated successfully!";

            return RedirectToAction("Profile");
        }
    }
}
