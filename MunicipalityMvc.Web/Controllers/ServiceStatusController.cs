using Microsoft.AspNetCore.Mvc;
using MunicipalityMvc.Core.Services;
using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Web.Controllers;

public class ServiceStatusController : Controller
{
	private readonly IServiceRequestStatusService _statusService;
	public ServiceStatusController(IServiceRequestStatusService statusService)
	{
		_statusService = statusService;
		
		var requests = SeedDataService.GetSampleRequests();
		_statusService.LoadRequests(requests);
	}

	[HttpGet]
	public IActionResult Index()
	{
		var requests = _statusService.GetAllRequests();
		return View(requests);
	}






	[HttpGet]
	public IActionResult Search(string requestNumber)
	{
		if (string.IsNullOrEmpty(requestNumber))
		{
			return View("SearchResult", null);
		}

		var request = _statusService.FindByRequestNumber(requestNumber);
		return View("SearchResult", request);
	}

	[HttpGet]
	public IActionResult Priority()
	{
		var requests = _statusService.GetPriorityRequests();
		return View(requests);
	}
}
