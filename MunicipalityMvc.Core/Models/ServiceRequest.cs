namespace MunicipalityMvc.Core.Models;

public enum RequestStatus
{
	New,
	InProgress,
	Assigned,
	Resolved,
	Closed
}

public enum RequestPriority
{
	Low = 3,
	Medium = 2,
	High = 1
}

public class ServiceRequest
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string RequestNumber { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public string Location { get; set; } = string.Empty;
	public string ReportedBy { get; set; } = string.Empty;
	public string ContactInfo { get; set; } = string.Empty;
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	
	public RequestStatus Status { get; set; } = RequestStatus.New;
	public RequestPriority Priority { get; set; } = RequestPriority.Medium;
	public string AssignedTo { get; set; } = string.Empty;
	public DateTime? StatusUpdatedDate { get; set; }
	public List<Guid> Dependencies { get; set; } = new();
}

