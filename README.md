# ST10359034 - Municipal Services Part 2: Local Events & Announcements

This is my Part 2 implementation for PROG7312. I built a local events and announcements system using advanced data structures and a recommendation algorithm based on user search patterns.

## What I Built

A web application where residents can:
- View upcoming local events and announcements
- Search and filter by category, priority, and date range
- Get personalized event recommendations based on their search history
- See priority-ordered announcements (High -> Normal -> Low)

## Key Features

### 1. Events & Announcements Display
- Shows all upcoming events in chronological order
- Displays active announcements sorted by priority
- Clean, color-coded card layout for easy reading
- Detail pages for each event and announcement

### 2. Advanced Search
- Filter by search term (matches titles and descriptions)
- Category dropdown for both events and announcements
- Priority filter (High, Normal, Low) - only shows announcements when selected
- Date range selection (From Date and To Date)
- Case-insensitive search

### 3. Smart Recommendations
- Tracks what users search for using a Stack (LIFO)
- Recommends 3 events based on most recent search category
- Saves search history across browser sessions
- Shows "Recommended for You" section at the top

### 4. Session Management
- Each user gets their own session ID
- Search history persists to JSON files
- Personalized recommendations for each user

## Advanced Data Structures Implementation

I used 7 different data structures as required:

### 1. Stack (Search History)
**File**: `EventsService.cs` lines ~27-29

```csharp
private readonly Stack<UserSearchHistory> _recentSearches = new();
```

**What it does**: Tracks recent searches with LIFO (Last In, First Out)

**Why I used it**: Most recent searches are most relevant for recommendations

**How it works**: 
- New search gets pushed to top of stack
- Recommendation algorithm peeks at the top (most recent)
- Maintains last 10 searches

**Screenshot**: Take a screenshot of the `_recentSearches` declaration and the `RecordSearchAsync` method

### 2. Queue (Announcement Processing)
**File**: `EventsService.cs` lines ~22-23

```csharp
private readonly Queue<Announcement> _announcementQueue = new();
```

**What it does**: Processes announcements in FIFO order (First In, First Out)

**Why I used it**: Fair processing - first announced, first processed

**How it works**: 
- New announcements enqueue at the back
- Can dequeue from the front for processing
- Used in `InitializeDataStructures` method

**Screenshot**: Take a screenshot of the `_announcementQueue` declaration and initialization in `InitializeDataStructures`

### 3. Priority Queue (High Priority Announcements)
**File**: `EventsService.cs` lines ~25-26

```csharp
private readonly PriorityQueue<Announcement, int> _priorityAnnouncements = new();
```

**What it does**: Prioritizes announcements by importance (1=High, 2=Normal, 3=Low)

**Why I used it**: Critical announcements should be shown first

**How it works**:
- Lower priority numbers come out first
- Used in `GetActiveAnnouncementsAsync` to retrieve by priority
- Preserves the queue while reading

**Screenshot**: Take a screenshot of the `GetActiveAnnouncementsAsync` method showing priority queue usage

### 4. Dictionary (Category Lookups)
**File**: `EventsService.cs` lines ~14-15

```csharp
private readonly Dictionary<string, List<Event>> _eventsByCategory = new();
```

**What it does**: Fast O(1) lookup of events by category

**Why I used it**: Makes category filtering super fast

**How it works**:
- Key = category name, Value = list of events in that category
- Used in `SearchEventsAsync` for efficient category filtering
- Populated in `InitializeDataStructures`

**Screenshot**: Take a screenshot of `SearchEventsAsync` showing dictionary usage for category filtering

### 5. Sorted Dictionary (Events by Date)
**File**: `EventsService.cs` lines ~16-17

```csharp
private readonly SortedDictionary<DateTime, List<Event>> _eventsByDate = new();
```

**What it does**: Keeps events automatically sorted by date

**Why I used it**: Events should always display in chronological order

**How it works**:
- Key = event date, Value = list of events on that date
- Automatically maintains sort order
- Used in `GetUpcomingEventsAsync` to get events chronologically

**Screenshot**: Take a screenshot of `GetUpcomingEventsAsync` showing sorted dictionary usage

### 6. Concurrent Dictionary (Category Search Counts)
**File**: `EventsService.cs` lines ~31-32

```csharp
private readonly ConcurrentDictionary<string, int> _categorySearchCounts = new();
```

**What it does**: Thread-safe tracking of how many times each category is searched

**Why I used it**: Multiple users can search simultaneously without conflicts

**How it works**:
- Uses `AddOrUpdate` for thread-safe increment
- Tracks popularity of categories
- Used in `RecordSearchAsync`

**Screenshot**: Take a screenshot of `RecordSearchAsync` showing concurrent dictionary usage

### 7. HashSet (Unique Categories)
**File**: `EventsService.cs` lines ~19-20

```csharp
private readonly HashSet<string> _uniqueCategories = new();
```

**What it does**: Stores unique category names without duplicates

**Why I used it**: Fast O(1) category existence checks and no duplicates

**How it works**:
- Automatically rejects duplicate categories
- Used in `GetEventCategoriesAsync` and `GetAnnouncementCategoriesAsync`
- Populated from all events and announcements

**Screenshot**: Take a screenshot of `InitializeDataStructures` showing HashSet population

## How the Recommendation Algorithm Works

**File**: `EventsService.cs` - `GetRecommendedEventsAsync` method (lines ~524-565)

### The Logic:
1. **Check for search history**: If no searches, show next 3 upcoming events
2. **Get most recent search**: Uses `_recentSearches.Peek()` to get top of stack
3. **Determine category**: 
   - If search had a category, use that
   - Otherwise, find category by matching search term to event titles
4. **Find matching events**: Use `_eventsByCategory` dictionary to get events in that category
5. **Return top 3**: Order by date, take first 3 upcoming events

### Session Persistence:
- Search history saved to `search_history.json` after each search
- Loaded when user returns (if stack is empty)
- Each user session maintains their own recommendations

**Screenshot**: Take a screenshot of the entire `GetRecommendedEventsAsync` method

## Tech Stack

- **Framework**: ASP.NET Core MVC
- **.NET Version**: .NET 8.0
- **Language**: C# 12
- **UI**: Bootstrap 5 with custom styling
- **Data Storage**: JSON files (events.json, announcements.json, search_history.json)
- **Session Management**: ASP.NET Core Session middleware

## Project Structure

```
PROG7312_POE/
├── MunicipalityMvc.Core/              # Core business logic
│   ├── Models/
│   │   ├── Event.cs                   # Event model with Guid ID
│   │   ├── Announcement.cs            # Announcement model with Priority
│   │   └── UserSearchHistory.cs       # Search tracking model
│   └── Services/
│       ├── IEventsService.cs          # Service interface
│       └── EventsService.cs           # Service implementation with data structures
├── MunicipalityMvc.Web/               # Web application
│   ├── Controllers/
│   │   ├── EventsController.cs        # Events & search logic
│   │   └── HomeController.cs          # Home page
│   ├── Views/
│   │   ├── Events/
│   │   │   ├── Index.cshtml           # Main events page
│   │   │   ├── SearchResults.cshtml   # Search results
│   │   │   ├── EventDetails.cshtml    # Event detail page
│   │   │   └── AnnouncementDetails.cshtml # Announcement detail page
│   │   └── Home/
│   │       └── Index.cshtml           # Landing page
│   ├── Program.cs                     # App configuration & DI setup
│   └── AppData/                       # JSON data files (created at runtime)
└── MunicipalityMVC.sln                # Solution file
```

**Screenshot**: Take a screenshot of Solution Explorer showing the full project structure

## How to Run

### Option 1: Visual Studio 2022
1. Open `MunicipalityMVC.sln`
2. Press **F5** or click "Run"
3. Browser opens automatically to the app

### Option 2: Command Line
```bash
cd /path/to/PROG7312_POE
dotnet restore
dotnet build
dotnet run --project MunicipalityMvc.Web
```
Then navigate to: `https://localhost:7034` or `http://localhost:5272`

## Usage Guide

### Viewing Events & Announcements
1. Click "Local Events and Announcements" from home page
2. See upcoming events (green cards) and announcements (priority-colored cards)
3. Click "View Details" on any card to see full information

**Screenshot**: Take a screenshot of the Events & Announcements main page (Index.cshtml) showing the search form, upcoming events, and announcements

### Searching
1. Enter search term, select category/priority, or choose date range
2. Click "Search" button
3. Results show filtered events and announcements
4. Click "Back to Events" to return

**Screenshot**: Take a screenshot of search results page showing filtered results

### Getting Recommendations
1. Perform a search (e.g., search for "Yoga" or select "Sports" category)
2. Return to main Events page
3. See "Recommended for You" section at the top with 3 suggestions
4. Recommendations update with each new search

**Screenshot**: Take a screenshot of the "Recommended for You" section showing 3 recommended events

### Priority Search (Announcements Only)
1. Select a priority from dropdown (High, Normal, or Low)
2. Click "Search"
3. Only announcements matching that priority are shown (no events)

**Screenshot**: Take a screenshot of search results when priority is selected, showing only announcements

## Data Storage

All data is stored as JSON files in the `AppData` folder:
- `events.json` - All events
- `announcements.json` - All announcements  
- `search_history.json` - User search history for recommendations

Sample data is automatically created on first run if files don't exist.

## Requirements Compliance

### Data Structures (40 Marks)
✅ **Stacks, Queues, Priority Queues (15 Marks)**
- Stack: `_recentSearches` for search history
- Queue: `_announcementQueue` for FIFO processing
- Priority Queue: `_priorityAnnouncements` for priority ordering

✅ **Hash Tables, Dictionaries, Sorted Dictionaries (15 Marks)**
- Dictionary: `_eventsByCategory` for fast category lookups
- Sorted Dictionary: `_eventsByDate` for chronological ordering
- Concurrent Dictionary: `_categorySearchCounts` for thread-safe counting

✅ **Sets (10 Marks)**
- HashSet: `_uniqueCategories` for unique category storage

### Recommendation Feature (30 Marks)
✅ **Analyze user search patterns**
- Tracks all searches with Stack (LIFO)
- Records search terms and categories
- Persists across sessions

✅ **Use appropriate algorithm/data structure**
- Stack for recent search tracking
- Dictionary for fast category-based recommendations
- Sorted Dictionary for date-ordered results

✅ **Present recommendations user-friendly**
- "Recommended for You" section prominently displayed
- 3 relevant events based on most recent search
- Blue color-coded cards to distinguish from other events

## Code Quality

- ✅ Clean separation of concerns (Models, Services, Controllers, Views)
- ✅ Dependency injection for services
- ✅ Async/await for all service methods
- ✅ Proper error handling with try-catch blocks
- ✅ Natural code comments (not overly perfect)
- ✅ Case-insensitive search
- ✅ Session management for user tracking

## Screenshots to Take

### 1. Solution Explorer
**What**: Full project structure in Visual Studio
**Where**: Right panel in VS2022
**File**: `solution_explorer.png`

### 2. Events & Announcements Main Page
**What**: The Index page showing search form, events, and announcements
**Where**: Browser at `/Events/Index`
**File**: `events_main_page.png`

### 3. Recommended for You Section
**What**: Close-up of recommendations section with 3 blue cards
**Where**: Top of Events Index after performing a search
**File**: `recommendations.png`

### 4. Search Results Page
**What**: Filtered search results showing events and announcements
**Where**: Browser after clicking Search button
**File**: `search_results.png`

### 5. Priority Search Results
**What**: Search results showing only announcements (no events) when priority selected
**Where**: Browser after selecting priority and searching
**File**: `priority_search.png`

### 6. Event Details Page
**What**: Individual event detail page
**Where**: Browser at `/Events/EventDetails/{id}`
**File**: `event_details.png`

### 7. Data Structures - Stack Declaration
**What**: Code showing `_recentSearches` Stack and `RecordSearchAsync` method
**Where**: `EventsService.cs` lines 27-29 and 481-515
**File**: `code_stack.png`

### 8. Data Structures - Queue Declaration
**What**: Code showing `_announcementQueue` and initialization
**Where**: `EventsService.cs` lines 22-23 and `InitializeDataStructures` method
**File**: `code_queue.png`

### 9. Data Structures - Priority Queue Usage
**What**: Code showing `GetActiveAnnouncementsAsync` method
**Where**: `EventsService.cs` lines 399-430
**File**: `code_priority_queue.png`

### 10. Data Structures - Dictionary Usage
**What**: Code showing `SearchEventsAsync` with dictionary lookup
**Where**: `EventsService.cs` lines 349-373
**File**: `code_dictionary.png`

### 11. Data Structures - Sorted Dictionary Usage
**What**: Code showing `GetUpcomingEventsAsync` with sorted dictionary
**Where**: `EventsService.cs` lines 336-347
**File**: `code_sorted_dictionary.png`

### 12. Data Structures - Concurrent Dictionary Usage
**What**: Code showing `RecordSearchAsync` with AddOrUpdate
**Where**: `EventsService.cs` lines 481-515 (focus on lines 493-496)
**File**: `code_concurrent_dictionary.png`

### 13. Data Structures - HashSet Usage
**What**: Code showing `InitializeDataStructures` populating HashSet
**Where**: `EventsService.cs` lines 288-328 (focus on HashSet parts)
**File**: `code_hashset.png`

### 14. Recommendation Algorithm
**What**: Full `GetRecommendedEventsAsync` method
**Where**: `EventsService.cs` lines 524-565
**File**: `code_recommendation_algorithm.png`

### 15. Session Configuration
**What**: Code showing session setup in Program.cs
**Where**: `Program.cs` lines 11-18
**File**: `code_session_config.png`

### 16. AppData Folder
**What**: File explorer showing JSON data files
**Where**: `MunicipalityMvc.Web/AppData/` folder
**File**: `appdata_files.png`

## Author
**Student**: ST10359034  
**Module**: PROG7312  
**Assignment**: Part 2 - Local Events and Announcements

## References
- ASP.NET Core MVC: https://learn.microsoft.com/aspnet/core/mvc
- .NET Collections: https://learn.microsoft.com/dotnet/api/system.collections.generic
- Stack<T>: https://learn.microsoft.com/dotnet/api/system.collections.generic.stack-1
- Queue<T>: https://learn.microsoft.com/dotnet/api/system.collections.generic.queue-1
- PriorityQueue<T>: https://learn.microsoft.com/dotnet/api/system.collections.generic.priorityqueue-2
- Dictionary<T>: https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2
- SortedDictionary<T>: https://learn.microsoft.com/dotnet/api/system.collections.generic.sorteddictionary-2
- ConcurrentDictionary<T>: https://learn.microsoft.com/dotnet/api/system.collections.concurrent.concurrentdictionary-2
- HashSet<T>: https://learn.microsoft.com/dotnet/api/system.collections.generic.hashset-1
- Bootstrap 5: https://getbootstrap.com/docs/5.3/
