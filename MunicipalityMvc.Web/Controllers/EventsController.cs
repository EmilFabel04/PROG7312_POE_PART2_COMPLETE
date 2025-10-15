using Microsoft.AspNetCore.Mvc;
using MunicipalityMvc.Core.Models;
using MunicipalityMvc.Core.Services;

namespace MunicipalityMvc.Web.Controllers
{
    // controller for events
    public class EventsController : Controller
    {
        private readonly IEventsService _eventsService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventsService eventsService, ILogger<EventsController> logger)
        {
            _eventsService = eventsService;
            _logger = logger;
        }

        // main events page
        public async Task<IActionResult> Index()
        {
            // set user session for search history for recommendations
            _eventsService.SetUserSession(HttpContext.Session.Id);
            
            var upcomingEvents = await _eventsService.GetUpcomingEventsAsync();
            var activeAnnouncements = await _eventsService.GetActiveAnnouncementsAsync();
            var eventCategories = await _eventsService.GetEventCategoriesAsync();
            var announcementCategories = await _eventsService.GetAnnouncementCategoriesAsync();
            
            // only show recommendations if user has search history
            var recentSearches = await _eventsService.GetRecentSearchesAsync();
            
            // debug
            _logger.LogInformation($"Recent searches count: {recentSearches.Count()}");
            foreach (var search in recentSearches)
            {
                _logger.LogInformation($"Search: {search.SearchTerm}, Category: {search.Category}");
            }
            
            var recommendedEvents = recentSearches.Any() 
                ? await _eventsService.GetRecommendedEventsAsync() 
                : Enumerable.Empty<Event>();
            
            _logger.LogInformation($"Recommended events count: {recommendedEvents.Count()}");

            var viewModel = new EventsIndexViewModel
            {
                UpcomingEvents = upcomingEvents,
                ActiveAnnouncements = activeAnnouncements,
                EventCategories = eventCategories,
                AnnouncementCategories = announcementCategories,
                RecommendedEvents = recommendedEvents
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

        // search events
        [HttpPost]
        public async Task<IActionResult> Search(EventsSearchViewModel searchModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            // set user session for search history
            _eventsService.SetUserSession(HttpContext.Session.Id);

            // record search for recommendations
            if (!string.IsNullOrEmpty(searchModel.SearchTerm) || !string.IsNullOrEmpty(searchModel.Category))
            {
                await _eventsService.RecordSearchAsync(searchModel.SearchTerm ?? "", searchModel.Category);
            }

            // search events and announcements
            var events = await _eventsService.SearchEventsAsync(
                searchModel.SearchTerm, 
                searchModel.Category, 
                searchModel.FromDate, 
                searchModel.ToDate);

            var announcements = await _eventsService.SearchAnnouncementsAsync(
                searchModel.SearchTerm, 
                searchModel.Category, 
                searchModel.Priority,
                searchModel.FromDate,
                searchModel.ToDate);

            var eventCategories = await _eventsService.GetEventCategoriesAsync();
            var announcementCategories = await _eventsService.GetAnnouncementCategoriesAsync();

            var viewModel = new EventsSearchResultsViewModel
            {
                SearchModel = searchModel,
                Events = events,
                Announcements = announcements,
                EventCategories = eventCategories,
                AnnouncementCategories = announcementCategories
            };

            return View("SearchResults", viewModel);
        }

    }

    // view models
    public class EventsIndexViewModel
    {
        public IEnumerable<Event> UpcomingEvents { get; set; } = new List<Event>();
        public IEnumerable<Announcement> ActiveAnnouncements { get; set; } = new List<Announcement>();
        public IEnumerable<string> EventCategories { get; set; } = new List<string>();
        public IEnumerable<string> AnnouncementCategories { get; set; } = new List<string>();
        public IEnumerable<Event> RecommendedEvents { get; set; } = new List<Event>();
    }

    public class EventsSearchViewModel
    {
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Priority { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class EventsSearchResultsViewModel
    {
        public EventsSearchViewModel SearchModel { get; set; } = new();
        public IEnumerable<Event> Events { get; set; } = new List<Event>();
        public IEnumerable<Announcement> Announcements { get; set; } = new List<Announcement>();
        public IEnumerable<string> EventCategories { get; set; } = new List<string>();
        public IEnumerable<string> AnnouncementCategories { get; set; } = new List<string>();
    }
}