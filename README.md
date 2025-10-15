# ST10359034 - Municipal Services — Citizen Engagement (ASP.NET Core MVC, .NET 8)

This is my Part 2 implementation that builds on Part 1. I added an events and announcements system with some data structures we learned about, recommendations based on what users search for, and keeping track of user sessions. Now the app has both issue reporting and local events/announcements.

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

**NEW in Part 2:** Residents can now look at local events and announcements, search by category and date, and get suggestions based on what they searched before. I used some of the data structures we learned about to organize the data better.

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

### What I Added
- **Events & Announcements Page**: People can look at upcoming events and announcements
- **Search**: Filter by search term, category, priority, and date range
- **Recommendations**: Shows events based on what you searched before
- **Session Tracking**: Remembers what you searched across browser sessions
- **Priority Ordering**: Important announcements show first (High → Normal → Low)

### Data Structures I Used
- **Stack**: Keeps track of recent searches (last in, first out)
- **Queue**: Processes announcements in order (first in, first out)
- **Priority Queue**: Important announcements get processed first
- **Dictionary**: Fast lookups by category
- **Sorted Dictionary**: Events stay sorted by date automatically
- **Concurrent Dictionary**: Safe for multiple users searching at same time
- **HashSet**: Stores unique categories without duplicates

### Key Screens (Part 2)
**Events & Announcements Main Page**
- Shows upcoming events and active announcements
- Search form with different filters
- "Recommended for You" section with suggestions

**Search Results**
- Shows filtered events and announcements
- Clean layout for results
- Easy way to go back to main page

**Event/Announcement Details**
- Individual pages for each event and announcement
- Shows all the information
- Back button to main events page

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
- **Local Events and Announcements Page**: Made in `Views/Events/Index.cshtml` to show events and announcements.
- **Data Structures (40 Marks)**:
  - **Stacks, Queues, Priority Queues (15 Marks)**: Used Stack for search history, Queue for announcements, PriorityQueue for handling priorities in `EventsService.cs`.
  - **Hash Tables, Dictionaries, Sorted Dictionaries (15 Marks)**: Used Dictionary for category lookups, SortedDictionary for date ordering, ConcurrentDictionary for safe operations in `EventsService.cs`.
  - **Sets (10 Marks)**: Used HashSet for unique categories in `EventsService.cs`.
- **Recommendation Feature (30 Marks)**: Made recommendations using search history and popular categories in `EventsService.cs`.
- **Search Functionality**: Can filter by term, category, priority, and date range in `EventsController.cs`.
- **Session Management**: Tracks user sessions for recommendations in `Program.cs` and `EventsController.cs`.

## How I Used the Data Structures

### Stack (User Search History)
- **What it does**: Keeps track of recent searches (last in, first out)
- **Why I used it**: So the most recent searches get priority for recommendations
- **How it works**: When someone searches, it gets added to the top of the stack

### Queue (Announcement Processing)
- **What it does**: Processes announcements in order (first in, first out)
- **Why I used it**: Makes sure announcements are handled fairly in the order they come in
- **How it works**: New announcements go to the back, old ones get processed first

### Priority Queue (High Priority Announcements)
- **What it does**: Important announcements get processed first
- **Why I used it**: High priority announcements should show before normal ones
- **How it works**: Priority 1 (High), 2 (Normal), 3 (Low) - lower numbers go first

### Dictionary (Category Lookups)
- **What it does**: Fast lookups by category
- **Why I used it**: Makes searching by category much faster
- **How it works**: Each category points to a list of events in that category

### Sorted Dictionary (Date Ordering)
- **What it does**: Keeps events sorted by date automatically
- **Why I used it**: Events should always show in date order
- **How it works**: Uses the date as the key so it stays sorted

### Concurrent Dictionary (Category Counts)
- **What it does**: Tracks how popular each category is
- **Why I used it**: Safe for multiple users searching at the same time
- **How it works**: Counts how many times each category gets searched

### HashSet (Unique Categories)
- **What it does**: Stores unique category names
- **Why I used it**: No duplicate categories, fast operations
- **How it works**: Automatically removes duplicates when adding categories

## How the Recommendation System Works

### The Basic Idea
The recommendation system looks at what you searched before and what categories are popular to suggest events you might like.

### How It Works
1. **Tracks Your Searches**: Every time you search, it remembers what you looked for using a Stack (most recent first)
2. **Counts Popular Categories**: Keeps track of how often each category gets searched
3. **Finds Matches**: Looks for events that match your recent searches or categories
4. **Fallback**: If nothing matches your recent searches, it shows popular categories

### Session Persistence
- Your search history gets saved to files so it remembers across browser sessions
- Each user gets their own personalized suggestions
- The system learns what you like based on what you search for

## Tech Stack & Architecture
- .NET 8, ASP.NET Core MVC (no extra packages needed beyond what comes with .NET)
- Project `MunicipalityMvc.Core`: has the models and services
- Project `MunicipalityMvc.Web`: has the controllers, views, and main app setup
- Dependency Injection: `Program.cs` sets up the services and saves data to `AppData/data`

## Projects and Layout
- `MunicipalityMvc.Core`
  - `Models/IssueReport.cs`: stores issue ticket information
  - `Models/Event.cs`: stores event information like title, date, location, etc.
  - `Models/Announcement.cs`: stores announcement information with priority levels
  - `Models/UserSearchHistory.cs`: tracks what users search for recommendations
  - `Services/IIssueService.cs`, `Services/IssueService.cs`: handles issue reporting with queue
  - `Services/IEventsService.cs`, `Services/EventsService.cs`: handles events and announcements with data structures
- `MunicipalityMvc.Web`
  - `Controllers/ReportsController.cs`: handles issue reporting pages
  - `Controllers/EventsController.cs`: handles events and announcements pages
  - `Controllers/HomeController.cs`: handles main menu
  - `Views/Reports/`: issue reporting UI pages
  - `Views/Events/`: events and announcements UI pages
  - `Views/Home/Index.cshtml`: main menu page
  - `Views/Shared/_Layout.cshtml`: main layout template
  - `wwwroot/css/site.css`: styling
  - `AppData/`: where all the JSON files are stored (issues, events, announcements, search history)

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
1. Home → click "Local Events & Announcements"
2. Look at upcoming events and announcements on the main page
3. Use the search form to filter by:
   - Search term (title/description)
   - Category (Sports, Community, etc.)
   - Priority (High, Normal, Low for announcements)
   - Date range (From Date and To Date)
4. Click "Search" to see results
5. Click on any event or announcement title to see full details
6. The "Recommended for You" section shows suggestions based on what you searched before
7. Your search history is remembered across browser sessions

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
- Events storage: `EventsService` keeps events in memory and saves to `events.json`
- Announcements storage: Similar setup with `announcements.json`
- User search history: Each user gets their own file (`search_history_<sessionId>.json`) for recommendations
- Data structures: All the collections (Stack, Queue, etc.) are kept in memory and saved to JSON files
- Session management: Tracks users by session ID for personalized recommendations

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
1) `EventsController.Index` loads events and announcements from JSON files into the data structures
2) When users search, `EventsController.Search` calls `EventsService.RecordSearchAsync`
3) Search history gets added to Stack and category counts get updated
4) Recommendations are made using recent searches and popular categories
5) All data structures are saved to JSON files so they persist
6) User sessions are tracked so recommendations work across visits

![Data Folder – issues.json and ticket folders](docs/05_data_folder.png)

## Engagement & Accessibility

### Part 1: Issue Reporting
- Submission feedback: progress bar + encouraging toasts while preparing the ticket.
- Email/SMS update switches: Users can opt into updates (stored with the ticket). These preferences are persisted for later phases.
- Success feedback: one-time toast and optional browser notification (permission‑based) personalized with the submitter's name.

### Part 2: Events & Announcements
- Recommendations: Shows events based on what you searched before and popular categories
- Session persistence: Remembers your search history across browser sessions
- Search: Multiple filter options (term, category, priority, date range) for finding what you want
- Priority ordering: Important announcements show first (High → Normal → Low)
- Real-time search: Results show up right away when you search

### General
- Keyboard navigation and labels for inputs; responsive design.
- Clean, intuitive interface with consistent styling across all features.

## Privacy & Security Notes

### Part 1: Issue Reporting
- Public list hides personal/contact info; shows only Ticket, Category, and Description.
- Input is trimmed server-side; unexpected notification permissions are not required.
- Files are stored locally; no external uploads.

### Part 2: Events & Announcements
- Your search history is stored locally per session; no sharing between users
- Session data expires after 30 minutes of inactivity
- Search history files are created locally and not sent anywhere
- All event and announcement data is public information

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
