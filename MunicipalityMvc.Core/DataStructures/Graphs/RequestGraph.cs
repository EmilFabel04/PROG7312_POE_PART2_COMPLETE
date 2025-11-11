using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.DataStructures.Graphs;

// Small graph that stores "A depends on B" links between requests.
// I use it to show what a request needs first and who is waiting on it.
public class RequestGraph
{
	private Dictionary<Guid, ServiceRequest> requests;
	private Dictionary<Guid, List<Guid>> adjacencyList;

	// construct empty graph
	public RequestGraph()
	{
		requests = new Dictionary<Guid, ServiceRequest>();
		adjacencyList = new Dictionary<Guid, List<Guid>>();
	}

	// add a request node and its dependency edges
	public void AddRequest(ServiceRequest request)
	{
		if (!requests.ContainsKey(request.Id))
		{
			requests[request.Id] = request;
			adjacencyList[request.Id] = new List<Guid>();
		}

		foreach (var depId in request.Dependencies)
		{
			if (!adjacencyList[request.Id].Contains(depId))
			{
				adjacencyList[request.Id].Add(depId);
			}
		}
	}

	// get request by id, or null
	public ServiceRequest GetRequest(Guid id)
	{
		return requests.ContainsKey(id) ? requests[id] : null;
	}

	// get direct dependencies, one hop
	public List<ServiceRequest> GetDependencies(Guid requestId)
	{
		var deps = new List<ServiceRequest>();
		
		if (!adjacencyList.ContainsKey(requestId))
			return deps;

		foreach (var depId in adjacencyList[requestId])
		{
			if (requests.ContainsKey(depId))
				deps.Add(requests[depId]);
		}

		return deps;
	}

	// get all dependencies using DFS
	public List<ServiceRequest> GetAllDependenciesDFS(Guid requestId)
	{
		var result = new List<ServiceRequest>();
		var visited = new HashSet<Guid>();
		DFS(requestId, visited, result);
		return result;
	}

	// internal DFS helper
	private void DFS(Guid requestId, HashSet<Guid> visited, List<ServiceRequest> result)
	{
		if (visited.Contains(requestId) || !adjacencyList.ContainsKey(requestId))
			return;

		visited.Add(requestId);

		foreach (var depId in adjacencyList[requestId])
		{
			if (requests.ContainsKey(depId))
			{
				result.Add(requests[depId]);
				DFS(depId, visited, result);
			}
		}
	}
/*
	// breadth first alternative, not used by UI right now
	public List<ServiceRequest> GetAllDependenciesBFS(Guid requestId)
	{
		var result = new List<ServiceRequest>();
		var visited = new HashSet<Guid>();
		var queue = new Queue<Guid>();

		if (!adjacencyList.ContainsKey(requestId))
			return result;

		queue.Enqueue(requestId);
		visited.Add(requestId);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();

			if (adjacencyList.ContainsKey(current))
			{
				foreach (var depId in adjacencyList[current])
				{
					if (!visited.Contains(depId) && requests.ContainsKey(depId))
					{
						visited.Add(depId);
						result.Add(requests[depId]);
						queue.Enqueue(depId);
					}
				}
			}
		}
		return result;
	}
*/
	// find requests that depend on the given id
	public List<ServiceRequest> GetRequestsDependingOn(Guid requestId)
	{
		var dependents = new List<ServiceRequest>();

		foreach (var kvp in adjacencyList)
		{
			if (kvp.Value.Contains(requestId) && requests.ContainsKey(kvp.Key))
			{
				dependents.Add(requests[kvp.Key]);
			}
		}
		return dependents;
	}

	// count of nodes in graph
	public int GetRequestCount()
	{
		return requests.Count;
	}

	// all nodes as a list
	public List<ServiceRequest> GetAllRequests()
	{
		return requests.Values.ToList();
	}

	// clear entire graph
	public void Clear()
	{
		requests.Clear();
		adjacencyList.Clear();
	}
}
