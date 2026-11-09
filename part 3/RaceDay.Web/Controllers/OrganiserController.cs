using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.Web.Models;
using RaceDay.Web.Services;
using System.Text.Json;

namespace RaceDay.Web.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly RaceDayApiService _apiService;

        private const long MaxFileSize = 5 * 1024 * 1024;

        private static readonly string[] AllowedFileTypes =
        {
            "image/jpeg",
            "image/png",
            "application/pdf"
        };

        public OrganiserController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetString("Role") == "Organiser";
        }

        public IActionResult Dashboard()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            ViewBag.OrganiserName = HttpContext.Session.GetString("Name");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Enrolments(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var enrolments =
                await _apiService.GetAsync<List<EventEnrolment>>(
                    $"api/events/{id}/enrolments");

            var model = new CaptureResultViewModel
            {
                EventId = id,
                Enrolments = enrolments ?? new List<EventEnrolment>()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CaptureResult(int eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var enrolments =
                await _apiService.GetAsync<List<EventEnrolment>>(
                    $"api/events/{eventId}/enrolments");

            var model = new CaptureResultViewModel
            {
                EventId = eventId,
                Enrolments = enrolments ?? new List<EventEnrolment>()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CaptureResult(
            CaptureResultViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (model.EnrolmentId <= 0 || model.Position <= 0 ||
                string.IsNullOrWhiteSpace(model.FinishTime))
            {
                ModelState.AddModelError("", "Complete all result fields.");
            }

            if (!ModelState.IsValid)
            {
                model.Enrolments =
                    await _apiService.GetAsync<List<EventEnrolment>>(
                        $"api/events/{model.EventId}/enrolments")
                    ?? new List<EventEnrolment>();

                return View(model);
            }

            var request = new CreateResultRequest
            {
                EnrolmentId = model.EnrolmentId,
                FinishTime = model.FinishTime,
                Position = model.Position
            };

            using var response = await _apiService.PostAsync(
                "api/results", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Could not save the result. Please check the details.");

                model.Enrolments =
                    await _apiService.GetAsync<List<EventEnrolment>>(
                        $"api/events/{model.EventId}/enrolments")
                    ?? new List<EventEnrolment>();

                return View(model);
            }

            TempData["Message"] = "Result captured successfully!";

            return RedirectToAction(
                "Enrolments", new { id = model.EventId });
        }

        // GET: Create Event
        [HttpGet]
        public async Task<IActionResult> CreateEvent()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var model = new EventFormViewModel
            {
                EventDate = DateTime.Now,
                EventTypes = await _apiService.GetAsync<List<EventType>>(
                    "api/event-types") ?? new List<EventType>()
            };

            return View(model);
        }

        // POST: Create Event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(EventFormViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                model.EventTypes = await _apiService.GetAsync<List<EventType>>(
                    "api/event-types") ?? new List<EventType>();

                return View(model);
            }

            var request = new
            {
                name = model.Name,
                description = model.Description,
                eventDate = model.EventDate,
                location = model.Location,
                distanceKm = model.DistanceKm,
                eventTypeId = model.EventTypeId
            };

            using var response = await _apiService.PostAsync(
                "api/events", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Could not create the event. Please check the details.");

                model.EventTypes = await _apiService.GetAsync<List<EventType>>(
                    "api/event-types") ?? new List<EventType>();

                return View(model);
            }

            TempData["Message"] = "Event created successfully!";
            return RedirectToAction("Index", "Events");
        }

        // GET: Edit Event
        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var events = await _apiService.GetAsync<List<Event>>(
                "api/events");

            var selectedEvent = events?.FirstOrDefault(e => e.Id == id);

            if (selectedEvent == null)
                return NotFound();

            var eventTypes = await _apiService.GetAsync<List<EventType>>(
                "api/event-types") ?? new List<EventType>();

            var model = new EventFormViewModel
            {
                Id = selectedEvent.Id,
                Name = selectedEvent.Name,
                Description = selectedEvent.Description,
                EventDate = selectedEvent.EventDate,
                Location = selectedEvent.Location,
                DistanceKm = selectedEvent.DistanceKm,
                EventTypeId = eventTypes.FirstOrDefault(
                    t => t.Name == selectedEvent.EventType)?.Id ?? 0,
                EventTypes = eventTypes
            };

            return View(model);
        }

        // POST: Edit Event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(
            int id, EventFormViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                model.EventTypes = await _apiService.GetAsync<List<EventType>>(
                    "api/event-types") ?? new List<EventType>();

                return View(model);
            }

            var request = new
            {
                name = model.Name,
                description = model.Description,
                eventDate = model.EventDate,
                location = model.Location,
                distanceKm = model.DistanceKm,
                eventTypeId = model.EventTypeId
            };

            using var response = await _apiService.PutAsync(
                $"api/events/{id}", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Could not update the event. Please check the details.");

                model.EventTypes = await _apiService.GetAsync<List<EventType>>(
                    "api/event-types") ?? new List<EventType>();

                return View(model);
            }

            TempData["Message"] = "Event updated successfully!";
            return RedirectToAction("Index", "Events");
        }

        // POST: Delete Event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            using var response = await _apiService.DeleteAsync(
                $"api/events/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not delete the event.";
            }
            else
            {
                TempData["Message"] = "Event deleted successfully!";
            }

            return RedirectToAction("Index", "Events");
        }

        // GET: View Categories for an Event
        [HttpGet]
        public async Task<IActionResult> Categories(int eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var categories = await _apiService.GetAsync<List<Category>>(
                $"api/categories?eventId={eventId}");

            ViewBag.EventId = eventId;

            return View(categories ?? new List<Category>());
        }

        // GET: Create Category
        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var model = new CategoryFormViewModel
            {
                Events = await _apiService.GetAsync<List<Event>>(
                    "api/events") ?? new List<Event>()
            };

            return View(model);
        }

        // POST: Create Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(
            CategoryFormViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                model.Events = await _apiService.GetAsync<List<Event>>(
                    "api/events") ?? new List<Event>();

                return View(model);
            }

            var request = new
            {
                name = model.Name,
                description = model.Description,
                raceEventId = model.RaceEventId
            };

            using var response = await _apiService.PostAsync(
                "api/categories", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Could not create the category. Please check the details.");

                model.Events = await _apiService.GetAsync<List<Event>>(
                    "api/events") ?? new List<Event>();

                return View(model);
            }

            TempData["Message"] = "Category created successfully!";

            return RedirectToAction(
                "Categories", new { eventId = model.RaceEventId });
        }

        // GET: Edit Category
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id, int eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var categories = await _apiService.GetAsync<List<Category>>(
                $"api/categories?eventId={eventId}");

            var selectedCategory = categories?.FirstOrDefault(
                c => c.Id == id);

            if (selectedCategory == null)
                return NotFound();

            var model = new CategoryFormViewModel
            {
                Id = selectedCategory.Id,
                Name = selectedCategory.Name,
                Description = selectedCategory.Description,
                RaceEventId = selectedCategory.RaceEventId,
                Events = await _apiService.GetAsync<List<Event>>(
                    "api/events") ?? new List<Event>()
            };

            return View(model);
        }

        // POST: Edit Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(
            int id, CategoryFormViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                model.Events = await _apiService.GetAsync<List<Event>>(
                    "api/events") ?? new List<Event>();

                return View(model);
            }

            var request = new
            {
                name = model.Name,
                description = model.Description,
                raceEventId = model.RaceEventId
            };

            using var response = await _apiService.PutAsync(
                $"api/categories/{id}", request);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("",
                    "Could not update the category. Please check the details.");

                model.Events = await _apiService.GetAsync<List<Event>>(
                    "api/events") ?? new List<Event>();

                return View(model);
            }

            TempData["Message"] = "Category updated successfully!";

            return RedirectToAction(
                "Categories", new { eventId = model.RaceEventId });
        }

        // POST: Delete Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(
            int id, int eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            using var response = await _apiService.DeleteAsync(
                $"api/categories/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not delete the category.";
            }
            else
            {
                TempData["Message"] = "Category deleted successfully!";
            }

            return RedirectToAction(
                "Categories", new { eventId });
        }

        // GET: Upload File
        [HttpGet]
        public IActionResult UploadFile()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            return View(new FileUploadViewModel());
        }

        // POST: Upload File to Azure through the API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(
            FileUploadViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (model.File == null || model.File.Length == 0)
            {
                ModelState.AddModelError(
                    "File", "Please select a file.");
            }
            else
            {
                if (model.File.Length > MaxFileSize)
                {
                    ModelState.AddModelError(
                        "File", "The file cannot exceed 5 MB.");
                }

                if (!AllowedFileTypes.Contains(
                    model.File.ContentType.ToLowerInvariant()))
                {
                    ModelState.AddModelError(
                        "File", "Only JPEG, PNG and PDF files are allowed.");
                }
            }

            if (!ModelState.IsValid)
                return View(model);

            using var response = await _apiService.UploadFileAsync(
                "api/files/upload", model.File!);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "The upload failed. Please check the file and try again.");

                return View(model);
            }

            var responseBody =
                await response.Content.ReadAsStringAsync();

            using var json = JsonDocument.Parse(responseBody);

            if (json.RootElement.TryGetProperty(
                "fileUrl", out var url))
            {
                model.FileUrl = url.GetString();
            }

            TempData["Message"] = "File uploaded successfully!";

            return View(model);
        }
    }
}
