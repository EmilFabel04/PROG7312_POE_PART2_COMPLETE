using MunicipalityMvc.Core.DataStructures.Trees;
using MunicipalityMvc.Core.DataStructures.Heaps;
using MunicipalityMvc.Core.DataStructures.Graphs;
using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services;

public class ServiceRequestStatusService : IServiceRequestStatusService
{
	private readonly SearchTree _searchTree;
	private readonly PriorityHeap _priorityHeap;
	private readonly RequestGraph _graph;

	// construct and wire up the structures
	public ServiceRequestStatusService()
	{
		_searchTree = new SearchTree();
		_priorityHeap = new PriorityHeap();
		_graph = new RequestGraph();
	}

	// load/refresh seed requests into all structures
	public void LoadRequests(List<ServiceRequest> requests)
	{
		_searchTree.Clear();
		_priorityHeap.Clear();
		_graph.Clear();

		foreach (var request in requests)
		{
			_searchTree.Add(request);
			_priorityHeap.Add(request);
			_graph.AddRequest(request);
		}
	}

	// lookup by request number (BST)
	public ServiceRequest FindByRequestNumber(string requestNumber)
	{
		return _searchTree.Find(requestNumber);
	}

	// all requests (sorted by key)
	public List<ServiceRequest> GetAllRequests()
	{
		return _searchTree.GetAll();
	}

	// requests ordered by priority (heap)
	public List<ServiceRequest> GetPriorityRequests()
	{
		return _priorityHeap.GetAll();
	}

	// upstream dependencies for a request (graph)
	public List<ServiceRequest> GetDependencies(Guid requestId)
	{
		return _graph.GetAllDependenciesDFS(requestId);
	}

	// who depends on this request (graph)
	public List<ServiceRequest> GetRequestsDependingOn(Guid requestId)
	{
		return _graph.GetRequestsDependingOn(requestId);
	}
}
