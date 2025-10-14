using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MunicipalityMvc.Web.Models;
using MunicipalityMvc.Core.Services;

namespace MunicipalityMvc.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEventsService _eventsService;

    public HomeController(ILogger<HomeController> logger, IEventsService eventsService)
    {
        _logger = logger;
        _eventsService = eventsService;
    }

    public async Task<IActionResult> Index()
    {
        var upcomingEvents = await _eventsService.GetUpcomingEventsAsync();
        var activeAnnouncements = await _eventsService.GetActiveAnnouncementsAsync();
        
        ViewBag.RecentEvents = upcomingEvents.Take(3);
        ViewBag.RecentAnnouncements = activeAnnouncements.Take(3);
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
