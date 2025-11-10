using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.DataStructures.Heaps;

public class PriorityHeap
{
	private List<ServiceRequest> heap;

	public PriorityHeap()
	{
		heap = new List<ServiceRequest>();
	}

	public void Add(ServiceRequest request)
	{
		heap.Add(request);
		HeapifyUp(heap.Count - 1);
	}

	public ServiceRequest GetNext()
	{
		if (heap.Count == 0)
			return null;

		var top = heap[0];
		heap[0] = heap[heap.Count - 1];
		heap.RemoveAt(heap.Count - 1);

		if (heap.Count > 0)
			HeapifyDown(0);

		return top;
	}

	public ServiceRequest Peek()
	{
		if (heap.Count == 0)
			return null;

		return heap[0];
	}

	public List<ServiceRequest> GetAll()
	{
		return new List<ServiceRequest>(heap);
	}

	public int Size()
	{
		return heap.Count;
	}

	public void Clear()
	{
		heap.Clear();
	}

	private void HeapifyUp(int index)
	{
		while (index > 0)
		{
			int parent = (index - 1) / 2;

			if (Compare(heap[index], heap[parent]) >= 0)
				break;

			Swap(index, parent);
			index = parent;
		}
	}

	private void HeapifyDown(int index)
	{
		while (true)
		{
			int left = 2 * index + 1;
			int right = 2 * index + 2;
			int smallest = index;

			if (left < heap.Count && Compare(heap[left], heap[smallest]) < 0)
				smallest = left;

			if (right < heap.Count && Compare(heap[right], heap[smallest]) < 0)
				smallest = right;

			if (smallest == index)
				break;

			Swap(index, smallest);
			index = smallest;
		}
	}

	private int Compare(ServiceRequest a, ServiceRequest b)
	{
		int priorityCompare = ((int)a.Priority).CompareTo((int)b.Priority);
		
		if (priorityCompare != 0)
			return priorityCompare;

		return a.CreatedDate.CompareTo(b.CreatedDate);
	}

	private void Swap(int i, int j)
	{
		var temp = heap[i];
		heap[i] = heap[j];
		heap[j] = temp;
	}
}

