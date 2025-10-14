using Microsoft.AspNetCore.Mvc;
using MunicipalityMvc.Core.Models;
using MunicipalityMvc.Core.Services;

namespace MunicipalityMvc.Web.Controllers
{
    // controller for events
    public class EventsController : Controller
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        // main events page
        public async Task<IActionResult> Index()
        {
            var upcomingEvents = await _eventsService.GetUpcomingEventsAsync();
            var activeAnnouncements = await _eventsService.GetActiveAnnouncementsAsync();

            var viewModel = new EventsIndexViewModel
            {
                UpcomingEvents = upcomingEvents,
                ActiveAnnouncements = activeAnnouncements
            };

            return View(viewModel);
        }

        // view event details
        public async Task<IActionResult> EventDetails(Guid id)
        {
            var evt = await _eventsService.GetEventByIdAsync(id);
            if (evt == null)
            {
                return NotFound();
            }

            return View(evt);
        }

        // view announcement details  
        public async Task<IActionResult> AnnouncementDetails(Guid id)
        {
            var announcement = await _eventsService.GetAnnouncementByIdAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

    }

    // view model
    public class EventsIndexViewModel
    {
        public IEnumerable<Event> UpcomingEvents { get; set; } = new List<Event>();
        public IEnumerable<Announcement> ActiveAnnouncements { get; set; } = new List<Announcement>();
    }
}