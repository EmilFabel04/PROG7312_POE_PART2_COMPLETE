# ST10359034 - Municipal Services — Citizen Engagement (ASP.NET Core MVC, .NET 8)

This is my Part 2 implementation building on Part 1. I added a comprehensive events and announcements system with advanced data structures, smart recommendations, and session-based user tracking. The app now includes both issue reporting and local events/announcements functionality.

## Contents
- Overview
- Part 2 Features (Events & Announcements)
- Requirements matrix (how the brief is met)
- Advanced Data Structures Implementation
- Tech stack & architecture
- Projects and file layout
- How to run (VS2022 and CLI)
- Usage guide (step-by-step)
- Data handling & persistence
- Recommendation Algorithm
- Engagement & accessibility
- Privacy & security notes
- Troubleshooting

## Overview
Residents can submit issues with location, category, description, and multiple attachments. The app stores tickets in a queue (FIFO) and displays the submitter's queue position on the success screen. An optional name/surname and contact details can be provided along with update preferences (email/SMS). The UI is responsive and polished.

**NEW in Part 2:** Residents can now browse local events and announcements, search by category and date, and receive personalized recommendations based on their search history. The system uses advanced data structures for efficient data organization and retrieval.

### Key Screens
Home (Main menu)

![Home – Main Menu](docs/01_home.png)

Report Issue (Create)

![Report Issue – Create](docs/02_report_create.png)

Report Success (Details with queue position)

![Report Success](docs/03_report_success.png)

All Reports (Public list)

![All Reports](docs/04_reports_list.png)

## Part 2 Features (Events & Announcements)

### New Functionality
- **Events & Announcements Page**: Browse upcoming events and active announcements
- **Advanced Search**: Filter by search term, category, priority, and date range
- **Smart Recommendations**: Personalized event suggestions based on user search history
- **Session Tracking**: User preferences remembered across browser sessions
- **Priority Ordering**: Announcements displayed High → Normal → Low priority

### Advanced Data Structures Used
- **Stack**: User search history (LIFO) for recent search tracking
- **Queue**: Announcement processing (FIFO) for fair handling
- **Priority Queue**: High-priority announcements processed first
- **Dictionary**: Category-based event lookups for fast searching
- **Sorted Dictionary**: Events automatically sorted by date
- **Concurrent Dictionary**: Thread-safe category popularity tracking
- **HashSet**: Unique category storage with no duplicates

### Key Screens (Part 2)
Events & Announcements Main Page
- Shows upcoming events and active announcements
- Search form with multiple filters
- "Recommended for You" section with personalized suggestions

Search Results
- Displays filtered events and announcements
- Clean, organized results layout
- Easy navigation back to main page

Event/Announcement Details
- Individual detail pages for each event and announcement
- Complete information display
- Back navigation to main events page

## Requirements Matrix

### Part 1 Requirements (Issue Reporting)
- Main menu with three tasks; only "Report Issues" enabled: Implemented in `Views/Home/Index.cshtml`.
- Report Issues form fields: location (TextBox), category (Dropdown), description (RichTextBox equivalent), attachments (OpenFileDialog/upload): Implemented in `Views/Reports/Create.cshtml`.
- Submit button and navigation back: Implemented in `Create.cshtml` with a back link and a submit button.
- Engagement feature: animated progress bar, rotating encouragement toasts during submission, one‑time success toast + optional browser notification: `Create.cshtml` and `Success.cshtml`.
- Data structure: queue for issues with JSON persistence: `MunicipalityMvc.Core/Services/IssueService.cs`.
- Responsiveness and consistent design: Bootstrap + custom theme in `wwwroot/css/site.css` and `_Layout.cshtml`.
- Privacy in public list: only ticket, category, description; no contact/location: `Views/Reports/Index.cshtml`.

### Part 2 Requirements (Events & Announcements)
- **Local Events and Announcements Page**: Implemented in `Views/Events/Index.cshtml` with comprehensive event and announcement display.
- **Advanced Data Structures (40 Marks)**:
  - **Stacks, Queues, Priority Queues (15 Marks)**: Stack for search history, Queue for announcements, PriorityQueue for priority handling in `EventsService.cs`.
  - **Hash Tables, Dictionaries, Sorted Dictionaries (15 Marks)**: Dictionary for category lookups, SortedDictionary for date ordering, ConcurrentDictionary for thread-safe operations in `EventsService.cs`.
  - **Sets (10 Marks)**: HashSet for unique categories in `EventsService.cs`.
- **Additional Recommendation Feature (30 Marks)**: Smart recommendation algorithm using search history and popular categories in `EventsService.cs`.
- **Search Functionality**: Filter by term, category, priority, and date range in `EventsController.cs`.
- **Session Management**: User session tracking for personalized recommendations in `Program.cs` and `EventsController.cs`.

## Advanced Data Structures Implementation

### Stack (User Search History)
```csharp
private readonly Stack<UserSearchHistory> _recentSearches = new();
```
- **Purpose**: LIFO (Last In, First Out) for tracking recent user searches
- **Usage**: Most recent searches are prioritized for recommendations
- **Implementation**: `RecordSearchAsync()` pushes searches, `GetRecommendedEventsAsync()` uses recent searches

### Queue (Announcement Processing)
```csharp
private readonly Queue<Announcement> _announcementQueue = new();
```
- **Purpose**: FIFO (First In, First Out) for fair announcement processing
- **Usage**: Ensures announcements are processed in order received
- **Implementation**: `InitializeDataStructures()` enqueues active announcements

### Priority Queue (High Priority Announcements)
```csharp
private readonly PriorityQueue<Announcement, int> _priorityAnnouncements = new();
```
- **Purpose**: High-priority announcements processed first
- **Usage**: Priority 1 (High), 2 (Normal), 3 (Low)
- **Implementation**: Priority-based enqueueing in `InitializeDataStructures()`

### Dictionary (Category Lookups)
```csharp
private readonly Dictionary<string, List<Event>> _eventsByCategory = new();
```
- **Purpose**: O(1) category-based event retrieval
- **Usage**: Fast filtering by category in search operations
- **Implementation**: Populated in `InitializeDataStructures()`, used in search methods

### Sorted Dictionary (Date Ordering)
```csharp
private readonly SortedDictionary<DateTime, List<Event>> _eventsByDate = new();
```
- **Purpose**: Automatic chronological ordering of events
- **Usage**: Events always displayed in date order
- **Implementation**: Date-based key organization in `InitializeDataStructures()`

### Concurrent Dictionary (Thread-Safe Category Counts)
```csharp
private readonly ConcurrentDictionary<string, int> _categorySearchCounts = new();
```
- **Purpose**: Thread-safe tracking of category popularity
- **Usage**: Multiple users can search simultaneously without conflicts
- **Implementation**: `AddOrUpdate()` for atomic operations in `RecordSearchAsync()`

### HashSet (Unique Categories)
```csharp
private readonly HashSet<string> _uniqueCategories = new();
```
- **Purpose**: Efficient storage of unique category names
- **Usage**: No duplicate categories, O(1) operations
- **Implementation**: Automatic uniqueness in `InitializeDataStructures()`

## Recommendation Algorithm

### How It Works
The recommendation system uses a combination of user search history and popular categories to suggest relevant events:

1. **Search History Tracking**: Every search is recorded using a Stack (LIFO) to prioritize recent searches
2. **Category Popularity**: ConcurrentDictionary tracks how often each category is searched
3. **Smart Matching**: Algorithm matches recent search terms and categories to find relevant events
4. **Fallback Strategy**: If no recent searches match, uses popular categories as recommendations

### Implementation Details
```csharp
// Record user searches
public async Task RecordSearchAsync(string searchTerm, string? category = null)
{
    var search = new UserSearchHistory { SearchTerm = searchTerm, Category = category };
    _recentSearches.Push(search); // Stack for LIFO
    _categorySearchCounts.AddOrUpdate(category, 1, (key, value) => value + 1); // Thread-safe counting
}

// Generate recommendations
public async Task<IEnumerable<Event>> GetRecommendedEventsAsync()
{
    // Use recent searches first (Stack LIFO)
    var recentSearches = _recentSearches.ToArray().Take(5);
    
    // Match events based on recent search terms and categories
    foreach (var search in recentSearches)
    {
        var matchingEvents = _events.Where(e => 
            e.Title.Contains(search.SearchTerm) || 
            e.Category == search.Category);
        recommendedEvents.AddRange(matchingEvents);
    }
    
    // Fallback to popular categories if no matches
    if (!recommendedEvents.Any())
    {
        var popularCategories = _categorySearchCounts.OrderByDescending(x => x.Value);
        // Use popular categories for recommendations
    }
}
```

### Session Persistence
- User search history is saved to JSON files per session ID
- Recommendations persist across browser sessions
- Each user gets personalized suggestions based on their search patterns

## Tech Stack & Architecture
- .NET 8, ASP.NET Core MVC (no external NuGet packages required beyond shared framework).
- Project `MunicipalityMvc.Core`: domain models (`IssueReport`), service contract (`IIssueService`), and implementation (`IssueService`).
- Project `MunicipalityMvc.Web`: MVC app; controllers, views, `Program.cs` DI configuration.
- Dependency Injection: `Program.cs` registers `IIssueService` as a singleton, persisting data under `AppData/data`.

## Projects and Layout
- `MunicipalityMvc.Core`
  - `Models/IssueReport.cs`: ticket fields; includes optional `FirstName`, `LastName`, `Email`, `Phone`, and `WantsEmailUpdates`, `WantsSmsUpdates`.
  - `Models/Event.cs`: event model with title, description, date, location, category, and recurring flag.
  - `Models/Announcement.cs`: announcement model with title, description, date, expiry, category, priority, and active status.
  - `Models/UserSearchHistory.cs`: tracks user search terms, categories, and timestamps for recommendations.
  - `Services/IIssueService.cs`, `Services/IssueService.cs`: queue-based storage, JSON persistence, attachment handling, queue position.
  - `Services/IEventsService.cs`, `Services/EventsService.cs`: events and announcements management with advanced data structures and recommendation algorithm.
- `MunicipalityMvc.Web`
  - `Controllers/ReportsController.cs`: Create, Success, and Index actions.
  - `Controllers/EventsController.cs`: Events and announcements display, search, and recommendation handling.
  - `Controllers/HomeController.cs`: Main menu and navigation.
  - `Views/Reports/{Create,Success,Index}.cshtml`: Issue reporting UI pages.
  - `Views/Events/{Index,SearchResults,EventDetails,AnnouncementDetails}.cshtml`: Events and announcements UI pages.
  - `Views/Home/Index.cshtml`: Main menu with navigation to all features.
  - `Views/Shared/_Layout.cshtml`: global layout, toast container helpers.
  - `wwwroot/css/site.css`: theme/branding.
  - `AppData/data/`: issues.json and per-ticket attachment folders (created at runtime).
  - `AppData/`: events.json, announcements.json, and user search history files (created at runtime).

## How To Run
### Visual Studio 2022
1. Prerequisites
   - Visual Studio 2022 17.8 or later
   - Workload: “ASP.NET and web development” (includes .NET 8 SDK)
2. Unzip & open
   - Download the ZIP of this repository or from submission
   - Extract to a short path (e.g., `C:\Projects\MunicipalityMVC`) to avoid long path issues
   - Double‑click `MunicipalityMVC.sln` to open the solution in Visual Studio
3. Restore & select startup
   - VS will restore automatically (no third‑party packages are used)
   - In Solution Explorer, right‑click `MunicipalityMvc.Web` → Set as Startup Project
4. Run / debug
   - Press F5 (Debug) or Ctrl+F5 (Run without debugging)
   - Your browser will open to `https://localhost:<port>` (Kestrel or IIS Express)
5. Try the flow
   - Home → Report an Issue → Fill the form → Submit → Success (note ticket code and queue position)
   - Home → View All Reports → See public list (Ticket, Category, Location, Description)

### .NET CLI
```
dotnet build MunicipalityMVC.sln -c Release
dotnet run --project MunicipalityMvc.Web
```
The app will open at `http://localhost:<port>`.

### Running from a ZIP (quick checklist)
1) Extract the ZIP to a short path (avoid long OneDrive paths)
2) Open `MunicipalityMVC.sln` in VS2022
3) Set startup project to `MunicipalityMvc.Web`
4) Build, then F5

### Clone from GitHub 
1. Clone the repository
   - HTTPS:
     ```
     git clone <THIS_REPO_URL>.git
     ```
   - SSH:
     ```
     git clone git@github.com:<ORG_OR_USER>/<REPO>.git
     ```
2. Open in Visual Studio or CLI
   - Visual Studio: double‑click `MunicipalityMVC.sln`, set `MunicipalityMvc.Web` as startup, F5
   - CLI:
     ```
     dotnet build MunicipalityMVC.sln -c Release
     dotnet run --project MunicipalityMvc.Web
     ```
3. First run notes
   - Trust the dev HTTPS certificate if prompted
   - The app writes data to `MunicipalityMvc.Web/AppData/data`

## Usage Guide

### Part 1: Issue Reporting
1. Home → click "Report an Issue".
2. Optionally enter name/surname and contact info; choose email/SMS updates.
3. Enter location, select category, and provide a clear description.
4. Add one or more attachments. Submit.
5. Submission shows an animated progress bar and rotating messages, then redirects to Success.
6. Success shows your Ticket code, queue position, details, attachments, and a one‑time toast/notification.
7. Home → "View All Reports" lists tickets (ticket/category/description only).

### Part 2: Events & Announcements
1. Home → click "Local Events & Announcements".
2. Browse upcoming events and active announcements on the main page.
3. Use the search form to filter by:
   - Search term (title/description)
   - Category (Sports, Community, etc.)
   - Priority (High, Normal, Low for announcements)
   - Date range (From Date and To Date)
4. Click "Search" to see filtered results.
5. Click on any event or announcement title to view full details.
6. The "Recommended for You" section shows personalized suggestions based on your search history.
7. Your search preferences are remembered across browser sessions for better recommendations.

## Feature Walkthrough (with screenshots)
Below are the key features, each paired with a screenshot placeholder you can replace.

1) Main Menu (Home)
- What you see: Friendly hero, primary CTA to “Report an Issue”, cards for key actions.
- Why it matters: Meets brief by showing three tasks and enabling only Report Issues initially.

![Home – Main Menu](docs/01_home.png)

2) Report Issue (Create)
- Fields: Name + Surname (optional), Email + Phone (optional), Allow Email Updates, Allow SMS Updates, Location, Category, Description, Attachments (multi‑upload).
- UX: Clean card layout; validation; submission button shows spinner; animated progress bar runs with encouraging messages.

![Report Issue – Create](docs/02_report_create.png)

3) Proactive Feedback During Submit
- What happens: As soon as Submit is clicked, we show an animated progress bar and cycle helpful toasts (e.g., “Packing your report…”, “Saving to the queue…”). This both informs and motivates.
- Implementation: Lightweight front‑end timer, followed by a real submit; no server delay required.

![Submit – Progress & Toasts](docs/02a_submit_progress.png)

4) Success Page
- Shows: Ticket code (short, friendly), queue position, full details, attachment filenames.
- Feedback: One‑time toast (and browser notification if permitted) personalized with the submitter’s name.
- Privacy: Only the owner sees contact details here; the public list hides contact info.

![Report Success](docs/03_report_success.png)

5) Public Reports List
- Shows: Ticket, Category, Location, and Description only.
- Extras: Copy ticket to clipboard; client‑side search box.

![All Reports](docs/04_reports_list.png)

## Data Handling & Persistence

### Part 1: Issue Reporting
- Queue storage: `IssueService` keeps an in-memory `Queue<IssueReport>` and persists the full queue to `issues.json`.
- Attachments: copied into `AppData/data/<ticket-id>/` using original filenames; file list saved with the ticket.
- Contact info & preferences: Optional `FirstName`, `LastName`, `Email`, `Phone`, plus `WantsEmailUpdates` and `WantsSmsUpdates` are stored per ticket for future parts (e.g., sending updates).

### Part 2: Events & Announcements
- Events storage: `EventsService` maintains in-memory collections with JSON persistence to `events.json`.
- Announcements storage: Similar JSON persistence to `announcements.json`.
- User search history: Individual JSON files per session ID (`search_history_<sessionId>.json`) for personalized recommendations.
- Advanced data structures: All collections (Stack, Queue, PriorityQueue, Dictionary, SortedDictionary, ConcurrentDictionary, HashSet) are maintained in memory and synchronized with JSON files.
- Session management: User sessions tracked via `HttpContext.Session.Id` for recommendation personalization.

### General
- No database server required; runs offline.
- All data persisted as JSON files in the `AppData/` directory.

### Data Flow (end-to-end)

#### Part 1: Issue Reporting
1) `ReportsController.Create (POST)` receives form fields and attachments.
2) Files are staged with original names and passed to `IssueService.AddAsync`.
3) `IssueService` copies files into the ticket folder, enqueues the report, and re‑writes `issues.json`.
4) The browser is redirected to Success, which reads the ticket and computes queue position.
5) The public list reads the whole queue as a summary.

#### Part 2: Events & Announcements
1) `EventsController.Index` loads events and announcements from JSON files into advanced data structures.
2) User searches are recorded via `EventsController.Search` → `EventsService.RecordSearchAsync`.
3) Search history is pushed to Stack and category counts updated in ConcurrentDictionary.
4) Recommendations generated using recent searches (Stack LIFO) and popular categories.
5) All data structures synchronized with JSON files for persistence.
6) Session-based user tracking maintains personalized recommendations across visits.

![Data Folder – issues.json and ticket folders](docs/05_data_folder.png)

## Engagement & Accessibility

### Part 1: Issue Reporting
- Submission feedback: progress bar + encouraging toasts while preparing the ticket.
- Email/SMS update switches: Users can opt into updates (stored with the ticket). These preferences are persisted for later phases.
- Success feedback: one-time toast and optional browser notification (permission‑based) personalized with the submitter's name.

### Part 2: Events & Announcements
- Smart recommendations: Smart suggestions based on user search history and popular categories.
- Session persistence: User preferences and search history remembered across browser sessions.
- Advanced search: Multiple filter options (term, category, priority, date range) for precise results.
- Priority ordering: Important announcements displayed first (High → Normal → Low).
- Real-time search: Instant filtering and results display.

### General
- Keyboard navigation and labels for inputs; responsive design.
- Clean, intuitive interface with consistent styling across all features.

## Privacy & Security Notes

### Part 1: Issue Reporting
- Public list hides personal/contact info; shows only Ticket, Category, and Description.
- Input is trimmed server-side; unexpected notification permissions are not required.
- Files are stored locally; no external uploads.

### Part 2: Events & Announcements
- User search history is stored locally per session ID; no cross-user data sharing.
- Session data is temporary and expires after 30 minutes of inactivity.
- Search history files are created locally and not transmitted externally.
- All event and announcement data is public information; no privacy concerns.

### General
- All data stored locally in JSON files; no external database connections.
- No user authentication required; anonymous usage supported.
- HTTPS enforced in production for secure data transmission.

## Troubleshooting
- .NET 8 SDK / VS version issues
  - Install .NET 8 SDK and VS2022 17.8+; close and reopen the solution
- HTTPS developer certificate prompt
  - When VS asks to trust the developer certificate, click Yes; or browse with `http://localhost:<port>` if you prefer
- Access denied writing to `MunicipalityMvc.Web/AppData/data`
  - Move the solution to a writeable folder (e.g., `C:\Projects\MunicipalityMVC`) and avoid controlled/roaming paths
- Port conflicts or stale debug sessions
  - Stop all instances, close browsers, then run again; confirm the port in the address bar
- Browser notifications not showing
  - They are optional; the app still shows in‑app toasts if you deny permission
- Long path names / file copy errors
  - Extract the ZIP to a short folder name; keep project depth minimal

## Code Highlights 
Add screenshots of the following key areas to demonstrate the queue‑based design. Use the filenames shown (at repo root or under `docs/` if you prefer a folder). Filenames are case‑sensitive on GitHub.

- Queue data structure and JSON persistence (mandatory emphasis)
  - File: `MunicipalityMvc.Core/Services/IssueService.cs`
  - Highlights: private `Queue<IssueReport> _queue`, constructor loading queue from JSON, `AddAsync` enqueuing and persisting, `GetPositionAsync`, `PersistQueue()`
  - Screenshot filename: `06_code_queue_issue_service.png`
  - Example:
    ![IssueService – Queue + Load](docs/06_code_queue_issue_service.png)

- Domain model with contact details and preferences
  - File: `MunicipalityMvc.Core/Models/IssueReport.cs`
  - Highlights: `TicketCode`, `FirstName`, `LastName`, `Email`, `Phone`, `WantsEmailUpdates`, `WantsSmsUpdates`, `AttachmentPaths`
  - Screenshot filename: `09_code_issue_report_model.png`
  - Example:
    ![IssueReport – Model](docs/09_code_issue_report_model.png)

- Dependency Injection setup
  - File: `MunicipalityMvc.Web/Program.cs`
  - Highlights: registering `IIssueService` with base data folder, building and running app
  - Screenshot filename: `10_code_di_program.png`
  - Example:
    ![Program – DI](docs/10_code_di_program.png)

- Ticket submission and file staging
  - File: `MunicipalityMvc.Web/Controllers/ReportsController.cs` (Create POST)
  - Highlights: accepting contact + preferences, staging uploads with original filenames, constructing `IssueReport`, calling `AddAsync`
  - Screenshot filename: `code_reports_controller_create.png` (or update this link to your chosen name)
  - Example (using current image):
    ![ReportsController – Create](docs/11_code_reports_controller_create.png)

- User engagement during submission (progress + messages)
  - File: `MunicipalityMvc.Web/Views/Reports/Create.cshtml`
  - Highlights: animated progress bar, rotating toasts with staged delay
  - Screenshot filename: `code_create_progress.png`

- Success page feedback and privacy
  - File: `MunicipalityMvc.Web/Views/Reports/Success.cshtml`
  - Highlights: one-time toast/notification, queue position, ticket details
  - Screenshot filename: `code_success_toast.png`

- Public reports list with privacy adjustments
  - File: `MunicipalityMvc.Web/Views/Reports/Index.cshtml`
  - Highlights: shows Ticket, Category, Location, Description only; copy-to-clipboard; search
  - Screenshot filename: `13_code_reports_index_privacy.png`
  - Example:
    ![Reports Index – Razor + JS](docs/13_code_reports_index_privacy.png)

- Enqueue + persist + bottom methods
  - File: `MunicipalityMvc.Core/Services/IssueService.cs`
  - Highlights: `AddAsync` (enqueue + persist); `GetPositionAsync`; `PersistQueue()`
  - Screenshot filename: `07_code_queue_add_persist.png`
  - Example:
    ![IssueService – Add & Persist](docs/07_code_queue_add_persist.png)

- Service contract
  - File: `MunicipalityMvc.Core/Services/IIssueService.cs`
  - Screenshot filename: `08_code_service_contract.png`
  - Example:
    ![IIssueService](docs/08_code_service_contract.png)

- Controller success action
  - File: `MunicipalityMvc.Web/Controllers/ReportsController.cs`
  - Screenshot filename: `12_code_reports_controller_success.png`
  - Example:
    ![ReportsController – Success](docs/12_code_reports_controller_success.png)

## Detailed code walkthrough
- Issue lifecycle (queue)
  - New tickets are enqueued and the queue is serialized to `AppData/data/issues.json` for durability
  - Queue position is computed from the current order and displayed on the success page
- Attachments
  - Uploaded files are copied to `AppData/data/<ticket-id>/` using their original filenames; the paths are saved with the ticket
- Contact & preferences
  - Optional `FirstName`, `LastName`, `Email`, `Phone`, and opt‑in switches `WantsEmailUpdates`/`WantsSmsUpdates` are stored per ticket
- UI feedback (engagement)
  - Create page: animated progress bar + rotating toasts during a brief staged delay before submission completes
  - Success page: one‑time toast and optional browser notification personalised with the submitter’s name
- Privacy in public list
  - Public list shows only Ticket, Category, Location, and Description (no personal details)

### Screenshot tips
- Use VS2022 with a dark or light theme; zoom ~120% for readability.
- Crop to the specific methods/blocks listed above, including file name in the editor tab if possible.
- Keep each image under ~1MB for repository size.

## License & Acknowledgements
- Academic use for PROG7312 Part 1 submission.
- Reference brief repository: `https://github.com/ST10359034/MunicipalServicesApp_PROG7312_POE`.

## References
- ASP.NET Core MVC docs: `https://learn.microsoft.com/aspnet/core/mvc/overview?view=aspnetcore-8.0`
- ASP.NET Core file uploads: `https://learn.microsoft.com/aspnet/core/mvc/models/file-uploads?view=aspnetcore-8.0`
- Dependency Injection in ASP.NET Core: `https://learn.microsoft.com/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-8.0`
- System.Text.Json serialization: `https://learn.microsoft.com/dotnet/standard/serialization/system-text-json-overview`
- .NET Queue<T> class: `https://learn.microsoft.com/dotnet/api/system.collections.generic.queue-1`
- Bootstrap 5 documentation: `https://getbootstrap.com/docs/5.3/getting-started/introduction/`
- Bootstrap Toasts: `https://getbootstrap.com/docs/5.3/components/toasts/`
- MDN Web Docs – Notification API: `https://developer.mozilla.org/docs/Web/API/Notifications_API`
- MDN Web Docs – HTML forms basics: `https://developer.mozilla.org/docs/Learn/Forms/Your_first_form`
- W3Schools – HTML: `https://www.w3schools.com/html/`
- W3Schools – Bootstrap 5: `https://www.w3schools.com/bootstrap5/`
