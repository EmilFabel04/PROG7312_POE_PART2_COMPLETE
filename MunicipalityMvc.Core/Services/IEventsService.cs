using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services
{
    // interface for events service
    public interface IEventsService
    {
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
        Task<IEnumerable<Event>> SearchEventsAsync(string? searchTerm, string? category, DateTime? fromDate, DateTime? toDate);
        Task<Event?> GetEventByIdAsync(Guid id);
        Task UpdateEventViewCountAsync(Guid eventId);
        
        Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync();
        Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string? searchTerm, string? category, string? priority);
        Task<Announcement?> GetAnnouncementByIdAsync(Guid id);
        Task UpdateAnnouncementViewCountAsync(Guid announcementId);
        
        Task<IEnumerable<string>> GetEventCategoriesAsync();
        Task<IEnumerable<string>> GetAnnouncementCategoriesAsync();
    }
}