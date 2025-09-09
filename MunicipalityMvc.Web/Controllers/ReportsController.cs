using Microsoft.AspNetCore.Mvc;
using MunicipalityMvc.Core.Models;
using MunicipalityMvc.Core.Services;

namespace MunicipalityMvc.Web.Controllers;

public sealed class ReportsController : Controller
{
	private readonly IIssueService _issueService;

	public ReportsController(IIssueService issueService)
	{
		_issueService = issueService;
	}

	[HttpGet]
	public IActionResult Create()
	{
		ViewBag.Categories = Enum.GetNames(typeof(IssueCategory));
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(string location, IssueCategory category, string description, List<IFormFile>? attachments)
	{
		if (string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(description))
		{
			ModelState.AddModelError(string.Empty, "Location and Description are required.");
			ViewBag.Categories = Enum.GetNames(typeof(IssueCategory));
			return View();
		}

		var tempFiles = new List<string>();
		if (attachments != null)
		{
			foreach (var file in attachments)
			{
				if (file.Length <= 0) continue;
				var tempPath = Path.GetTempFileName();
				await using var stream = System.IO.File.OpenWrite(tempPath);
				await file.CopyToAsync(stream);
				tempFiles.Add(tempPath);
			}
		}

		var report = new IssueReport
		{
			Location = location.Trim(),
			Category = category,
			Description = description.Trim()
		};

		await _issueService.AddAsync(report, tempFiles);

		TempData["SuccessMessage"] = "Report submitted successfully.";
		return RedirectToAction(nameof(Success), new { id = report.Id });
	}

	[HttpGet]
	public async Task<IActionResult> Success(Guid id)
	{
		var report = await _issueService.GetByIdAsync(id);
		if (report is null) return RedirectToAction("Index", "Home");
		ViewBag.Position = await _issueService.GetPositionAsync(id);
		return View(report);
	}

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		var items = await _issueService.GetAllAsync();
		return View(items);
	}
}


