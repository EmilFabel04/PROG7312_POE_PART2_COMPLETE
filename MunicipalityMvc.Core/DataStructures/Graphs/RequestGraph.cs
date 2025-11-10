using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.DataStructures.Graphs;

public class RequestGraph
{
	private Dictionary<Guid, ServiceRequest> requests;
	private Dictionary<Guid, List<Guid>> adjacencyList;

	public RequestGraph()
	{
		requests = new Dictionary<Guid, ServiceRequest>();
		adjacencyList = new Dictionary<Guid, List<Guid>>();
	}

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

	public ServiceRequest GetRequest(Guid id)
	{
		return requests.ContainsKey(id) ? requests[id] : null;
	}

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

	public List<ServiceRequest> GetAllDependenciesDFS(Guid requestId)
	{
		var result = new List<ServiceRequest>();
		var visited = new HashSet<Guid>();
		DFS(requestId, visited, result);
		return result;
	}

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

	public int GetRequestCount()
	{
		return requests.Count;
	}

	public List<ServiceRequest> GetAllRequests()
	{
		return requests.Values.ToList();
	}

	public void Clear()
	{
		requests.Clear();
		adjacencyList.Clear();
	}
}
