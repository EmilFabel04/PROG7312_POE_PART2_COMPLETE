namespace MunicipalityMvc.Core.Models;

/// <summary>
/// Categories a resident can choose from when reporting an issue.
/// </summary>
public enum IssueCategory
{
	Sanitation,
	Roads,
	Utilities,
	Parks,
	Safety,
	Other
}

/// <summary>
/// Represents a single ticket reported by a resident. The app stores these in a
/// simple FIFO queue and persists them to JSON for Part 1 of the PoE.
/// </summary>
public sealed class IssueReport
{
	public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Short, human-friendly ticket code (e.g., MS-ABC123) generated on submit.
    /// </summary>
	public string TicketCode { get; set; } = string.Empty;
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public string? Phone { get; set; }
	public bool WantsEmailUpdates { get; set; }
	public bool WantsSmsUpdates { get; set; }

    /// <summary>
    /// Location text provided by the resident (street, landmark, etc.).
    /// </summary>
	public string Location { get; set; } = string.Empty;
	public IssueCategory Category { get; set; } = IssueCategory.Other;
	public string Description { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public List<string> AttachmentPaths { get; set; } = new();
}


