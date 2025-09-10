using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services;

// Abstraction for storing and retrieving resident issue reports.
// Part 1 uses a simple queue persisted to JSON.
public interface IIssueService
{
	// Return all reports in their current queue order.
	Task<IReadOnlyList<IssueReport>> GetAllAsync(CancellationToken cancellationToken = default);
	// Add a new report and copy attachments; persists the queue.
	Task<IssueReport> AddAsync(IssueReport report, IEnumerable<string> attachmentSourcePaths, CancellationToken cancellationToken = default);
    // Get a report by its unique Id.
    Task<IssueReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    // Return a 1-based queue position for the given report Id.
    Task<int?> GetPositionAsync(Guid id, CancellationToken cancellationToken = default);
}


