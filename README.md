# Municipal Services — Citizen Engagement (ASP.NET Core MVC, .NET 8)

This project implements Part 1 of the brief as a modern web app. It enables residents to report municipal issues with attachments and engagement features, using a queue data structure for storage and transparency.

Screenshots are referenced below — add your images to `docs/` with the listed filenames.

## Contents
- Overview
- Requirements matrix (how the brief is met)
- Tech stack & architecture
- Projects and file layout
- How to run (VS2022 and CLI)
- Usage guide (step-by-step)
- Data handling & persistence
- Engagement & accessibility
- Privacy & security notes
- Troubleshooting

## Overview
Residents can submit issues with location, category, description, and multiple attachments. The app stores tickets in a queue (FIFO) and displays the submitter’s queue position on the success screen. An optional name/surname and contact details can be provided along with update preferences (email/SMS). The UI is responsive and polished.

### Key Screens
- Home (Main menu) — `docs/01_home.png`
- Report Issue (Create) — `docs/02_report_create.png`
- Report Success (Details with queue position) — `docs/03_report_success.png`
- All Reports (Public list: ticket, category, description only) — `docs/04_reports_list.png`

## Requirements Matrix
- Main menu with three tasks; only “Report Issues” enabled: Implemented in `Views/Home/Index.cshtml`.
- Report Issues form fields: location (TextBox), category (Dropdown), description (RichTextBox equivalent), attachments (OpenFileDialog/upload): Implemented in `Views/Reports/Create.cshtml`.
- Submit button and navigation back: Implemented in `Create.cshtml` with a back link and a submit button.
- Engagement feature: animated progress bar, rotating encouragement toasts during submission, one‑time success toast + optional browser notification: `Create.cshtml` and `Success.cshtml`.
- Data structure: queue for issues with JSON persistence: `MunicipalityMvc.Core/Services/IssueService.cs`.
- Responsiveness and consistent design: Bootstrap + custom theme in `wwwroot/css/site.css` and `_Layout.cshtml`.
- Privacy in public list: only ticket, category, description; no contact/location: `Views/Reports/Index.cshtml`.

## Tech Stack & Architecture
- .NET 8, ASP.NET Core MVC (no external NuGet packages required beyond shared framework).
- Project `MunicipalityMvc.Core`: domain models (`IssueReport`), service contract (`IIssueService`), and implementation (`IssueService`).
- Project `MunicipalityMvc.Web`: MVC app; controllers, views, `Program.cs` DI configuration.
- Dependency Injection: `Program.cs` registers `IIssueService` as a singleton, persisting data under `AppData/data`.

## Projects and Layout
- `MunicipalityMvc.Core`
  - `Models/IssueReport.cs`: ticket fields; includes optional `FirstName`, `LastName`, `Email`, `Phone`, and `WantsEmailUpdates`, `WantsSmsUpdates`.
  - `Services/IIssueService.cs`, `Services/IssueService.cs`: queue-based storage, JSON persistence, attachment handling, queue position.
- `MunicipalityMvc.Web`
  - `Controllers/ReportsController.cs`: Create, Success, and Index actions.
  - `Views/Reports/{Create,Success,Index}.cshtml`: UI pages.
  - `Views/Shared/_Layout.cshtml`: global layout, toast container helpers.
  - `wwwroot/css/site.css`: theme/branding.
  - `AppData/data/`: issues.json and per-ticket attachment folders (created at runtime).

## How To Run
### Visual Studio 2022
1. Install VS2022 17.8+ with .NET 8 SDK and “ASP.NET and web development”.
2. Open `MunicipalityMVC.sln`.
3. Set startup project to `MunicipalityMvc.Web`.
4. Press F5.

### .NET CLI
```
dotnet build MunicipalityMVC.sln -c Release
dotnet run --project MunicipalityMvc.Web
```

The app will open at `http://localhost:xxxx`.

## Usage Guide
1. Home → click “Report an Issue”.
2. Optionally enter name/surname and contact info; choose email/SMS updates.
3. Enter location, select category, and provide a clear description.
4. Add one or more attachments. Submit.
5. Submission shows an animated progress bar and rotating messages, then redirects to Success.
6. Success shows your Ticket code, queue position, details, attachments, and a one‑time toast/notification.
7. Home → “View All Reports” lists tickets (ticket/category/description only).

## Data Handling & Persistence
- Queue storage: `IssueService` keeps an in-memory `Queue<IssueReport>` and persists the full queue to `issues.json`.
- Attachments: copied into `AppData/data/<ticket-id>/` using original filenames; file list saved with the ticket.
- No database server required; runs offline.

## Engagement & Accessibility
- Submission feedback: progress bar + encouraging toasts while preparing the ticket.
- Success feedback: one-time toast and optional browser notification (permission‑based).
- Keyboard navigation and labels for inputs; responsive design.

## Privacy & Security Notes
- Public list hides personal/contact info; shows only Ticket, Category, and Description.
- Input is trimmed server-side; unexpected notification permissions are not required.
- Files are stored locally; no external uploads.

## Troubleshooting
- “Can’t find .NET 8”: install .NET 8 SDK and ensure VS2022 17.8+.
- Browser notification blocked: the app still works; notifications are optional.
- Access denied writing to `AppData/data`: run VS as user with folder permissions or choose a writeable workspace.

## Code Highlights (add screenshots here)
Add screenshots of the following key areas to demonstrate implementation quality and the queue-based design. Save images under `docs/` with the suggested filenames.

- Queue data structure and JSON persistence (mandatory emphasis)
  - File: `MunicipalityMvc.Core/Services/IssueService.cs`
  - Highlights: private `Queue<IssueReport> _queue`, constructor loading queue from JSON, `AddAsync` enqueuing and persisting, `GetPositionAsync`, `PersistQueue()`
  - Screenshot: `docs/code_queue_issue_service.png`

- Domain model with contact details and preferences
  - File: `MunicipalityMvc.Core/Models/IssueReport.cs`
  - Highlights: `TicketCode`, `FirstName`, `LastName`, `Email`, `Phone`, `WantsEmailUpdates`, `WantsSmsUpdates`, `AttachmentPaths`
  - Screenshot: `docs/code_issue_report_model.png`

- Dependency Injection setup
  - File: `MunicipalityMvc.Web/Program.cs`
  - Highlights: registering `IIssueService` with base data folder, building and running app
  - Screenshot: `docs/code_di_program.png`

- Ticket submission and file staging
  - File: `MunicipalityMvc.Web/Controllers/ReportsController.cs` (Create POST)
  - Highlights: accepting contact + preferences, staging uploads with original filenames, constructing `IssueReport`, calling `AddAsync`
  - Screenshot: `docs/code_reports_controller_create.png`

- User engagement during submission (progress + messages)
  - File: `MunicipalityMvc.Web/Views/Reports/Create.cshtml`
  - Highlights: animated progress bar, rotating toasts with staged delay
  - Screenshot: `docs/code_create_progress.png`

- Success page feedback and privacy
  - File: `MunicipalityMvc.Web/Views/Reports/Success.cshtml`
  - Highlights: one-time toast/notification, queue position, ticket details
  - Screenshot: `docs/code_success_toast.png`

- Public reports list with privacy adjustments
  - File: `MunicipalityMvc.Web/Views/Reports/Index.cshtml`
  - Highlights: shows Ticket, Category, Location, Description only; copy-to-clipboard; search
  - Screenshot: `docs/code_reports_index_privacy.png`

### Screenshot tips
- Use VS2022 with a dark or light theme; zoom ~120% for readability.
- Crop to the specific methods/blocks listed above, including file name in the editor tab if possible.
- Keep each image under ~1MB for repository size.

## License & Acknowledgements
- Academic use for PROG7312 Part 1 submission.
- Reference brief repository: `https://github.com/ST10359034/MunicipalServicesApp_PROG7312_POE`.

