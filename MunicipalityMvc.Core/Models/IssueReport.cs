namespace MunicipalityMvc.Core.Models;

public enum IssueCategory
{
	Sanitation,
	Roads,
	Utilities,
	Parks,
	Safety,
	Other
}

public sealed class IssueReport
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string TicketCode { get; set; } = string.Empty;
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public string? Phone { get; set; }
	public bool WantsEmailUpdates { get; set; }
	public bool WantsSmsUpdates { get; set; }
	public string Location { get; set; } = string.Empty;
	public IssueCategory Category { get; set; } = IssueCategory.Other;
	public string Description { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public List<string> AttachmentPaths { get; set; } = new();
}


