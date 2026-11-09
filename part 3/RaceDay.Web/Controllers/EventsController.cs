using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly RaceDayApiService _apiService;

        public EventsController(RaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var events = await _apiService.GetAsync<List<Event>>(
                    "api/events");

                return View(events ?? new List<Event>());
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = ex.ToString();

                return View(new List<Event>());
            }
        }
    }
}