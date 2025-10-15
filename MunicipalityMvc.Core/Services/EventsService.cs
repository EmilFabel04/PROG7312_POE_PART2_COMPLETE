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
        
        // dictionary for category lookups ,makes searching faster
        private readonly Dictionary<string, List<Event>> _eventsByCategory = new();
        // sorted dictionary for events by date, keeps them in order
        private readonly SortedDictionary<DateTime, List<Event>> _eventsByDate = new();
        
        // hashset for categories
        private readonly HashSet<string> _uniqueCategories = new();
        
        // queue for announcement processing ,for first in first out
        private readonly Queue<Announcement> _announcementQueue = new();
        
        // priority queue for high priority announcements, important ones first
        private readonly PriorityQueue<Announcement, int> _priorityAnnouncements = new();
        
        // stack for user search history ,last in first out to get recent searches
        private readonly Stack<UserSearchHistory> _recentSearches = new();
        
        // concurrent dictionary for category counts
        private readonly ConcurrentDictionary<string, int> _categorySearchCounts = new();
        private string? _currentUserSession;
        
        public EventsService(string dataDirectory)
        {
            _dataDirectory = dataDirectory;
            Directory.CreateDirectory(_dataDirectory);
            LoadData();
            InitializeDataStructures();
        }

        /*
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
*/
        // load data from json files
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
                },
                new Event
                {
                    Title = "Book Club Meeting",
                    Description = "Monthly book club discussion at the library",
                    Date = DateTime.Today.AddDays(5),
                    Location = "Public Library",
                    Category = "Culture",
                    IsRecurring = true
                },
                new Event
                {
                    Title = "Fitness Bootcamp",
                    Description = "Outdoor fitness training for all skill levels",
                    Date = DateTime.Today.AddDays(4),
                    Location = "City Park",
                    Category = "Sports",
                    IsRecurring = true
                },
                new Event
                {
                    Title = "Cooking Class",
                    Description = "Learn to cook healthy meals on a budget",
                    Date = DateTime.Today.AddDays(12),
                    Location = "Community Kitchen",
                    Category = "Education",
                    IsRecurring = false
                },
                new Event
                {
                    Title = "Music Festival",
                    Description = "Annual music festival featuring local bands",
                    Date = DateTime.Today.AddDays(30),
                    Location = "Waterfront",
                    Category = "Culture",
                    IsRecurring = false
                },
                new Event
                {
                    Title = "Job Fair",
                    Description = "Connect with local employers and explore career opportunities",
                    Date = DateTime.Today.AddDays(15),
                    Location = "Convention Center",
                    Category = "Government",
                    IsRecurring = false
                },
                new Event
                {
                    Title = "Garden Workshop",
                    Description = "Tips and tricks for sustainable gardening",
                    Date = DateTime.Today.AddDays(8),
                    Location = "Botanical Gardens",
                    Category = "Education",
                    IsRecurring = false
                },
                new Event
                {
                    Title = "Movie Night",
                    Description = "Free outdoor movie screening for families",
                    Date = DateTime.Today.AddDays(6),
                    Location = "Recreation Center",
                    Category = "Community",
                    IsRecurring = true
                },
                new Event
                {
                    Title = "Yoga in the Park",
                    Description = "Free morning yoga sessions for all ages",
                    Date = DateTime.Today.AddDays(2),
                    Location = "Memorial Park",
                    Category = "Sports",
                    IsRecurring = true
                }
            };

            _events.AddRange(sampleEvents);
            SaveEvents();
        }

        // create sample announcements
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

        // initialize data structures by populating all the collections with data
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

        // get upcoming events using sorted dictionary
        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            var upcomingEvents = new List<Event>();
            var today = DateTime.Today;
            
            foreach (var kvp in _eventsByDate)
            {
        if (kvp.Key >= today)
                {
              upcomingEvents.AddRange(kvp.Value);
                }
            }
            return await Task.FromResult(upcomingEvents);
        }
        // search events using dictionary for category lookups
        public async Task<IEnumerable<Event>> SearchEventsAsync(string? searchTerm, string? category, DateTime? fromDate, DateTime? toDate)
        {
            IEnumerable<Event> results;

            // use dictionary for fast category lookup
            if (!string.IsNullOrWhiteSpace(category) && _eventsByCategory.ContainsKey(category))
            {
                results = _eventsByCategory[category];
            }
            else
            {
                results = _events;
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {results = results.Where(e => e.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                           e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            if (fromDate.HasValue)
            {
                results = results.Where(e => e.Date >= fromDate.Value);
            }
if (toDate.HasValue)
            {
                results = results.Where(e => e.Date <= toDate.Value);
            }
            
            return await Task.FromResult(results.OrderBy(e => e.Date));
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

        // get active announcements using priority queue
        public async Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync()
        {
            var activeAnnouncements = new List<Announcement>();
            var now = DateTime.UtcNow;
            var tempQueue = new PriorityQueue<Announcement, int>();
            
            // get all announcements from priority queue while preserving it
            while (_priorityAnnouncements.Count > 0)
            {
                _priorityAnnouncements.TryDequeue(out var announcement, out var priority);
                if (announcement != null)
                {tempQueue.Enqueue(announcement, priority);
                    if (announcement.IsActive && (announcement.ExpiryDate == null || announcement.ExpiryDate > now))
                    {
                activeAnnouncements.Add(announcement);
                    }
            }
            }
            // restore priority queue
            while (tempQueue.Count > 0)
            {
                tempQueue.TryDequeue(out var announcement, out var priority);
                if (announcement != null)
                {
         _priorityAnnouncements.Enqueue(announcement, priority);
                }
            }
            return await Task.FromResult(activeAnnouncements);
        }
        // search announcements
        public async Task<IEnumerable<Announcement>> SearchAnnouncementsAsync(string? searchTerm, string? category, string? priority, DateTime? fromDate = null, DateTime? toDate = null)
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
            if (fromDate.HasValue)
            {
                query = query.Where(a => a.Date >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(a => a.Date <= toDate.Value);
            }
            return await Task.FromResult(query
                .OrderBy(a => a.Priority == "High" ? 1 : a.Priority == "Normal" ? 2 : 3)
                .ThenByDescending(a => a.Date));
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
        
       
      
        public void SetUserSession(string sessionId)
        {
            _currentUserSession = sessionId;
            
            // always load search history if stack is empty
            if (_recentSearches.Count == 0)
            {
                LoadUserSearchHistory();
            }
        }
        
        public async Task RecordSearchAsync(string searchTerm, string? category = null)
        {
            var search = new UserSearchHistory
            {
                SearchTerm = searchTerm ?? "",
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
            
            SaveUserSearchHistory();
            await Task.CompletedTask;
        }
        
        // get recent searches from stack
        public async Task<IEnumerable<UserSearchHistory>> GetRecentSearchesAsync()
        {
            return await Task.FromResult(_recentSearches.ToArray());
        }
        
        // recommendation algorithm based on search history using stack
        public async Task<IEnumerable<Event>> GetRecommendedEventsAsync()
        {
            var upcomingEvents = _events.Where(e => e.Date >= DateTime.Today).ToList();
            
            // if no search history return upcoming events
            if (_recentSearches.Count == 0)
            {
                return await Task.FromResult(upcomingEvents.OrderBy(e => e.Date).Take(3));
            }
            
            
            var mostRecentSearch = _recentSearches.Peek();
            
           
            string categoryToUse = mostRecentSearch.Category;
            
            
            if (string.IsNullOrEmpty(categoryToUse) && !string.IsNullOrEmpty(mostRecentSearch.SearchTerm))
            {
                var matchedEvent = upcomingEvents
                    .FirstOrDefault(e => e.Title.Contains(mostRecentSearch.SearchTerm, StringComparison.OrdinalIgnoreCase));
                
                if (matchedEvent != null)
                {
                    categoryToUse = matchedEvent.Category;
                }
            }
            
           
            if (!string.IsNullOrEmpty(categoryToUse) && _eventsByCategory.ContainsKey(categoryToUse))
            {
                var categoryEvents = _eventsByCategory[categoryToUse]
                    .Where(e => e.Date >= DateTime.Today)
                    .OrderBy(e => e.Date)
                    .Take(3);
                
                return await Task.FromResult(categoryEvents);
            }
            
            // fallback to upcoming events
            return await Task.FromResult(upcomingEvents.OrderBy(e => e.Date).Take(3));
        }
       
        private void SaveUserSearchHistory()
        {
            try
            {
                var historyFile = Path.Combine(_dataDirectory, "search_history.json");
                var historyList = _recentSearches.ToList(); // convert stack to list to preserve order
                var historyJson = JsonSerializer.Serialize(historyList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(historyFile, historyJson);
            }
            catch
            {
                // ignore save errors
            }
        }
        
        private void LoadUserSearchHistory()
        {
            try
            {
                var historyFile = Path.Combine(_dataDirectory, "search_history.json");
                if (File.Exists(historyFile))
                {
                    var historyJson = File.ReadAllText(historyFile);
                    var historyList = JsonSerializer.Deserialize<List<UserSearchHistory>>(historyJson);
                    
                    if (historyList != null && historyList.Any())
                    {
                        // push in reverse order to maintain stack order 
                        for (int i = historyList.Count - 1; i >= 0; i--)
                        {
                            _recentSearches.Push(historyList[i]);
                        }
                    }
                }
            }
            catch
            {
                // ignore load errors
            }
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