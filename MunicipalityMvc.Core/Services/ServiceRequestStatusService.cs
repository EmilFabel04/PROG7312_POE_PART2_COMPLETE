using MunicipalityMvc.Core.DataStructures.Trees;
using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services;

public class ServiceRequestStatusService : IServiceRequestStatusService
{
	private readonly SearchTree _searchTree;

	public ServiceRequestStatusService()
	{
		_searchTree = new SearchTree();
	}

	public void LoadRequests(List<ServiceRequest> requests)
	{
		_searchTree.Clear();

		foreach (var request in requests)
		{
			_searchTree.Add(request);
		}
	}

	public ServiceRequest FindByRequestNumber(string requestNumber)
	{
		return _searchTree.Find(requestNumber);
	}

	public List<ServiceRequest> GetAllRequests()
	{
		return _searchTree.GetAll();
	}

	public List<ServiceRequest> GetPriorityRequests()
	{
		// will implement with heap later
		return new List<ServiceRequest>();
	}

	public List<ServiceRequest> GetDependencies(Guid requestId)
	{
		// will implement with graph later
		return new List<ServiceRequest>();
	}

	public List<ServiceRequest> GetRequestsDependingOn(Guid requestId)
	{
		// will implement with graph later
		return new List<ServiceRequest>();
	}
}
