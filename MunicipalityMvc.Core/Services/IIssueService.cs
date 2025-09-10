using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services;

/// <summary>
/// Abstraction for storing and retrieving resident issue reports.
/// The Part 1 implementation uses a simple queue persisted to JSON.
/// </summary>
public interface IIssueService
{
	/// <summary>Return all reports in their current queue order.</summary>
	Task<IReadOnlyList<IssueReport>> GetAllAsync(CancellationToken cancellationToken = default);
	/// <summary>Add a new report and copy attachments; persists the queue.</summary>
	Task<IssueReport> AddAsync(IssueReport report, IEnumerable<string> attachmentSourcePaths, CancellationToken cancellationToken = default);
    /// <summary>Get a report by its unique Id.</summary>
    Task<IssueReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>Return a 1-based queue position for the given report Id.</summary>
    Task<int?> GetPositionAsync(Guid id, CancellationToken cancellationToken = default);
}


