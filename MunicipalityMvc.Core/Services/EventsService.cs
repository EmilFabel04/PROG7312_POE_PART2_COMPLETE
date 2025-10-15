using MunicipalityMvc.Core.Models;
using System.Collections.Concurrent;
using System.Text.Json;

namespace MunicipalityMvc.Core.Services
{
    // events service
    public sealed class EventsService : IEventsService
    {
        private readonly string _dataDirectory;
        private readonly List<Event> _events = new();
        private readonly List<Announcement> _announcements = new();
        
        // dictionary  category lookups
        private readonly Dictionary<string, List<Event>> _eventsByCategory = new();
        // sorted dictionary for events by date
        private readonly SortedDictionary<DateTime, List<Event>> _eventsByDate = new();
        
        // hashset for categories
        private readonly HashSet<string> _uniqueCategories = new();
        
        // queue for announcement processing
        private readonly Queue<Announcement> _announcementQueue = new();
        
        // priority queue for high priority announcements
        private readonly PriorityQueue<Announcement, int> _priorityAnnouncements = new();
        
        // stack for user search history
        private readonly Stack<UserSearchHistory> _recentSearches = new();
        
        // concurrent dictionary for thread-safe category counts
        private readonly ConcurrentDictionary<string, int> _categorySearchCounts = new();
        public EventsService(string dataDirectory)
        {
            _dataDirectory = dataDirectory;
            Directory.CreateDirectory(_dataDirectory);
            LoadData();
            InitializeDataStructures();
        }

        // clear old data files
        private void ClearData()
        {
            var eventsFile = Path.Combine(_dataDirectory, "events.json");
            var announcementsFile = Path.Combine(_dataDirectory, "announcements.json");
            
            if (File.Exists(eventsFile))
            {
                File.Delete(eventsFile);
            }
            if (File.Exists(announcementsFile))
            {
                File.Delete(announcementsFile);
            }
        }

        // load data of json files
        private void LoadData()
        {
            var eventsFile = Path.Combine(_dataDirectory, "events.json");
            var announcementsFile = Path.Combine(_dataDirectory, "announcements.json");

            if (File.Exists(eventsFile))
            {
                var eventsJson = File.ReadAllText(eventsFile);
                if (!string.IsNullOrEmpty(eventsJson))
                {
                    var events = JsonSerializer.Deserialize<List<Event>>(eventsJson);
                    if (events != null)
                    {
                        _events.AddRange(events);
                    }
                }
            }
            else
            {
                CreateSampleEvents();
            }
            if (File.Exists(announcementsFile))
            {
                var announcementsJson = File.ReadAllText(announcementsFile);
                if (!string.IsNullOrEmpty(announcementsJson))
                {
                    var announcements = JsonSerializer.Deserialize<List<Announcement>>(announcementsJson);
                    if (announcements != null)
                    {
                        _announcements.AddRange(announcements);
                    }
                }
            }
            else
            {
                CreateSampleAnnouncements();
            }
        }
        // create demo events
        private void CreateSampleEvents()
        {
            var sampleEvents = new List<Event>
            {
                new Event
                {
                    Title = "Community Cleanup Day",
                    Description = "Join us for a community cleanup event in the park",
                    Date = DateTime.Today.AddDays(7),
                    Location = "Central Park",
                    Category = "Community",
                    IsRecurring = false
                },
                new Event
                {
                    Title = "Town Hall Meeting",
                    Description = "Monthly town hall meeting to discuss local issues",
                    Date = DateTime.Today.AddDays(14),
                    Location = "Town Hall",
                    Category = "Government",
                    IsRecurring = true
                },
                new Event
                {
                    Title = "Farmers Market",
                    Description = "Weekly farmers market with fresh local produce",
                    Date = DateTime.Today.AddDays(3),
                    Location = "Market Square",
                    Category = "Community",
                    IsRecurring = true
                },
                new Event
                {
                    Title = "Art Exhibition",
                    Description = "Local artists showcase their latest works",
                    Date = DateTime.Today.AddDays(21),
                    Location = "Community Center",
                    Category = "Culture",
                    IsRecurring = false
                },
                new Event
                {
                    Title = "Youth Sports Tournament",
                    Description = "Annual youth soccer and basketball tournament",
                    Date = DateTime.Today.AddDays(28),
                    Location = "Sports Complex",
                    Category = "Sports",
                    IsRecurring = true
                },
                new Event
                {
                    Title = "Emergency Preparedness Workshop",
                    Description = "Learn essential emergency preparedness skills",
                    Date = DateTime.Today.AddDays(10),
                    Location = "Fire Station",
                    Category = "Education",
                    IsRecurring = false
                }
            };

            _events.AddRange(sampleEvents);
            SaveEvents();
        }

        // create demo announcements
        private void CreateSampleAnnouncements()
        {
            var sampleAnnouncements = new List<Announcement>
            {
                new Announcement
                {
                 Title = "Road Closure Notice",
                    Description = "Main Street will be closed for construction from 8 AM to 5 PM",
                    Date = DateTime.Today,
                    Category = "Infrastructure",
                    Priority = "High",
                    ExpiryDate = DateTime.Today.AddDays(3)
                },
                new Announcement
                {
                    Title = "New Library Hours",
                    Description = "The library will now be open until 8 PM on weekdays",
                Date = DateTime.Today.AddDays(-1),
                    Category = "Services",
                    Priority = "Normal"
                },
                new Announcement
                {
                    Title = "Water Shutoff",
                    Description = "Scheduled water maintenance in the northern SUburbs",
                    Date = DateTime.Today.AddDays(2),
                    Category = "Infrastructure",
                    Priority = "High",
                    ExpiryDate = DateTime.Today.AddDays(3)
                },
                new Announcement
                {
                    Title = "Public Holiday Notice",
                    Description = "Municipal offices will be closed for public holiday",
                    Date = DateTime.Today.AddDays(5),
                    Category = "Government",
                    Priority = "Low"
                },
                new Announcement
                {
                    Title = "Recycling Program Update",
                    Description = "New recycling bins available at community centers",
                    Date = DateTime.Today.AddDays(-3),
                    Category = "Environment",
                    Priority = "Normal"
                },
                new Announcement
                {
                    Title = "School Zone Safety",
                    Description = "Reminder: Reduced speed limits in school zones during term time",
                    Date = DateTime.Today.AddDays(-2),
                    Category = "Safety",
                    Priority = "High"
                },
                new Announcement
                {
                    Title = "Community Garden Opening",
                    Description = "New community garden opens this weekend for residents",
                    Date = DateTime.Today.AddDays(1),
                    Category = "Community",
                    Priority = "Normal"
                }
            };

            _announcements.AddRange(sampleAnnouncements);
            SaveAnnouncements();
        }

        // initialize data structures
        private void InitializeDataStructures()
        {
            // populate events by category dictionary
            foreach (var evt in _events)
            {
                if (!_eventsByCategory.ContainsKey(evt.Category))
                {
                    _eventsByCategory[evt.Category] = new List<Event>();
                }
                _eventsByCategory[evt.Category].Add(evt);
            }
            
            // populate events by date sorted dictionary
            foreach (var evt in _events)
            {
                var dateKey = evt.Date.Date;
                if (!_eventsByDate.ContainsKey(dateKey))
                {
                    _eventsByDate[dateKey] = new List<Event>();
                }
                _eventsByDate[dateKey].Add(evt);
                
                // add to unique categories hashset
                _uniqueCategories.Add(evt.Category);
            }
            
            // populate announcement queue with active announcements
            foreach (var announcement in _announcements.Where(a => a.IsActive))
            {
                _announcementQueue.Enqueue(announcement);
                
                // add high priority announcements to priority queue
                if (announcement.Priority == "High")
                {
              _priorityAnnouncements.Enqueue(announcement, 1);
                }
                else if (announcement.Priority == "Normal")
                {
                    _priorityAnnouncements.Enqueue(announcement, 2);
                }
                else if (announcement.Priority == "Low")
                {
                    _priorityAnnouncements.Enqueue(announcement, 3);
                }
        }
        }

        // get upcoming events
        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            return await Task.FromResult(_events.Where(e => e.Date >= DateTime.Today).OrderBy(e => e.Date));
        }
        // search events
        public async Task<IEnumerable<Event>> SearchEventsAsync(string? searchTerm, string? category, DateTime? fromDate, DateTime? toDate)
        {
            var query = _events.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
             query = query.Where(e => e.Title.Contains(searchTerm) || e.Description.Contains(searchTerm));
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(e => e.Category == category);
            }
            if (fromDate.HasValue)
            {
                query = query.Where(e => e.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
             query = query.Where(e => e.Date <= toDate.Value);
            }
            return await Task.FromResult(query.OrderBy(e => e.Date));
        }

        // get event by id
        public async Task<Event?> GetEventByIdAsync(Guid id)
        {
        return await Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
        }

        // update view count
        public async Task UpdateEventViewCountAsync(Guid eventId)
        {
            var evt = _events.FirstOrDefault(e => e.Id == eventId);
            if (evt != null)
            {
                // view tracking can be added later
                SaveEvents();
            }
            await Task.CompletedTask;
        }

        // get active announcements
        public async Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync()
        {
            var now = DateTime.UtcNow;
            return await Task.FromResult(_announcements.Where(a => a.IsActive && 
                (a.ExpiryDate == null || a.ExpiryDate > now)).OrderByDescending(a => a.Date));
        }
        // search announcements
        public async Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string? searchTerm, string? category, string? priority)
        {
            var query = _announcements.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Title.Contains(searchTerm) || a.Description.Contains(searchTerm));
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(a => a.Category == category);
            }
            if (!string.IsNullOrWhiteSpace(priority))
            {
                query = query.Where(a => a.Priority == priority);
            }
            return await Task.FromResult(query.OrderByDescending(a => a.Date));
        }

        // kry announcement by id
        public async Task<Announcement?> GetAnnouncementByIdAsync(Guid id)
        {
            return await Task.FromResult(_announcements.FirstOrDefault(a => a.Id == id));
        }

        // update announcement view count
        public async Task UpdateAnnouncementViewCountAsync(Guid announcementId)
        {
            var announcement = _announcements.FirstOrDefault(a => a.Id == announcementId);
            if (announcement != null)
            {
                // view tracking can be added later
                SaveAnnouncements();
            }
            await Task.CompletedTask;
        }

        // get event categories using hashset
        public async Task<IEnumerable<string>> GetEventCategoriesAsync()
        {
            return await Task.FromResult(_uniqueCategories.OrderBy(c => c));
        }

        // get announcement categories
        public async Task<IEnumerable<string>> GetAnnouncementCategoriesAsync()
        {
            return await Task.FromResult(_announcements.Select(a => a.Category).Distinct().OrderBy(c => c));
        }
        
        // record search history using stack
        public async Task RecordSearchAsync(string searchTerm, string? category = null)
        {
            var search = new UserSearchHistory
            {
                SearchTerm = searchTerm,
                Category = category,
                SearchDate = DateTime.UtcNow
            };
            
            _recentSearches.Push(search);
            
            // update category search count using concurrent dictionary
            if (!string.IsNullOrEmpty(category))
            {
                _categorySearchCounts.AddOrUpdate(category, 1, (key, value) => value + 1);
            }
            
            // keep only last 10 searches
            if (_recentSearches.Count > 10)
            {
                var temp = new Stack<UserSearchHistory>();
                for (int i = 0; i < 10; i++)
                {
                    temp.Push(_recentSearches.Pop());
                }
                _recentSearches.Clear();
                while (temp.Count > 0)
                {
                    _recentSearches.Push(temp.Pop());
                }
            }
            await Task.CompletedTask;
        }
        
        // get recent searches from stack
        public async Task<IEnumerable<UserSearchHistory>> GetRecentSearchesAsync()
        {
            return await Task.FromResult(_recentSearches.ToArray());
        }
        
        // get popular categories using concurrent dictionary
        public async Task<IEnumerable<string>> GetPopularCategoriesAsync()
        {
            return await Task.FromResult(_categorySearchCounts
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .Take(5));
        }
        
        // recommendation algorithm based on search history
        public async Task<IEnumerable<Event>> GetRecommendedEventsAsync()
        {
            // get most searched categories
            var popularCategories = _categorySearchCounts
                .OrderByDescending(x => x.Value)
                .Select(x => x.Key)
                .Take(3)
                .ToList();
            
            if (!popularCategories.Any())
            {
                // if no search history, return upcoming events
                return await GetUpcomingEventsAsync();
            }
            // find events in popular categories
            var recommendedEvents = new List<Event>();
            foreach (var category in popularCategories)
            {
                if (_eventsByCategory.ContainsKey(category))
                {
                 recommendedEvents.AddRange(_eventsByCategory[category]
                        .Where(e => e.Date >= DateTime.Today)
                        .Take(3));
                }
            }
            return await Task.FromResult(recommendedEvents.Distinct().OrderBy(e => e.Date));
        }
        // save events
        private void SaveEvents()
        {
            var eventsFile = Path.Combine(_dataDirectory, "events.json");
            var eventsJson = JsonSerializer.Serialize(_events, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(eventsFile, eventsJson);
        }
        // save announcements
        private void SaveAnnouncements()
        {
            var announcementsFile = Path.Combine(_dataDirectory, "announcements.json");
            var announcementsJson = JsonSerializer.Serialize(_announcements, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(announcementsFile, announcementsJson);
        }
 }
}