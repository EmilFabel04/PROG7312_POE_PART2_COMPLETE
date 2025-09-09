using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services;

public interface IIssueService
{
	Task<IReadOnlyList<IssueReport>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<IssueReport> AddAsync(IssueReport report, IEnumerable<string> attachmentSourcePaths, CancellationToken cancellationToken = default);
}


