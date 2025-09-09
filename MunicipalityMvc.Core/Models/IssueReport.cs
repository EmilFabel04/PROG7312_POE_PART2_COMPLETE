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
	public string Location { get; set; } = string.Empty;
	public IssueCategory Category { get; set; } = IssueCategory.Other;
	public string Description { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public List<string> AttachmentPaths { get; set; } = new();
}


