using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services
{
    // interface for events service
    public interface IEventsService
    {
        // event methods
        Task<IEnumerable<Event>> GetUpcomingEventsAsync(); // get future events
        Task<IEnumerable<Event>> SearchEventsAsync(string? searchTerm, string? category, DateTime? fromDate, DateTime? toDate); // search events
        Task<Event?> GetEventByIdAsync(Guid id); // get specific event
        
        // announcement methods
        Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync(); // get active announcements
        Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string? searchTerm, string? category, string? priority, DateTime? fromDate = null, DateTime? toDate = null); // search announcements
        Task<Announcement?> GetAnnouncementByIdAsync(Guid id); // get specific announcement
        
        // category methods
        Task<IEnumerable<string>> GetEventCategoriesAsync(); // get all event categories
        Task<IEnumerable<string>> GetAnnouncementCategoriesAsync(); // get all announcement categories
        
        // search history with stack
        void SetUserSession(string sessionId); // set user session
        Task RecordSearchAsync(string searchTerm, string? category = null); // record what user searched
        Task<IEnumerable<UserSearchHistory>> GetRecentSearchesAsync(); // get recent searches
        
        // popular categories using concurrent dictionary
        Task<IEnumerable<string>> GetPopularCategoriesAsync(); // get most searched categories
        
        // recommendation algorithm
        Task<IEnumerable<Event>> GetRecommendedEventsAsync(); // get recommended events
    }
}