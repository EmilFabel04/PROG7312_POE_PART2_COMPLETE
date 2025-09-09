# Municipal Services — MVC (Part 1)

ASP.NET Core MVC (.NET 8) reimplementation of the Windows Forms brief. Targets Visual Studio 2022 and provides the Part 1 feature: Report Issues.

Reference spec adapted from the WinForms version in `MunicipalServicesApp_PROG7312_POE` repository. See the original brief-like implementation here: https://github.com/ST10359034/MunicipalServicesApp_PROG7312_POE

## Projects
- `MunicipalityMvc.Core` — Models and `IIssueService`/`IssueService` for JSON persistence
- `MunicipalityMvc.Web` — MVC web app with controllers and views

## Features Implemented
- Main menu with three options; only Report Issues enabled
- Report page: location, category, description, file attachments (multi-upload)
- Engagement label and progress indicator
- Persistence: JSON file under `AppData/data` with copied attachments

## Run (Visual Studio 2022)
1. Open `MunicipalityMVC.sln`.
2. Set startup project to `MunicipalityMvc.Web`.
3. Build and Run (F5). Requires .NET 8 SDK/workload installed.

## File Locations
- Data: `<solution root>/MunicipalityMvc.Web/AppData/data/issues.json`
- Attachments: per-report folder under the same `data` directory

## Notes
- The two other menu options are disabled for later parts.
- Controller: `ReportsController` handles create and success screens.
