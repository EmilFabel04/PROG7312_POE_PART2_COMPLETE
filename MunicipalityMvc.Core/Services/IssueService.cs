using System.Text.Json;
using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services;

public sealed class IssueService : IIssueService
{
	private readonly string _dataDirectory;
	private readonly string _dbFilePath;
	private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

	public IssueService(string baseDataDirectory)
	{
		_dataDirectory = Path.Combine(baseDataDirectory, "data");
		Directory.CreateDirectory(_dataDirectory);
		_dbFilePath = Path.Combine(_dataDirectory, "issues.json");
		if (!File.Exists(_dbFilePath)) File.WriteAllText(_dbFilePath, "[]");
	}

	public async Task<IReadOnlyList<IssueReport>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		await using var stream = File.OpenRead(_dbFilePath);
		return await JsonSerializer.DeserializeAsync<List<IssueReport>>(stream, _jsonOptions, cancellationToken) ?? new();
	}

	public async Task<IssueReport> AddAsync(IssueReport report, IEnumerable<string> attachmentSourcePaths, CancellationToken cancellationToken = default)
	{
		var attachmentsFolder = Path.Combine(_dataDirectory, report.Id.ToString());
		Directory.CreateDirectory(attachmentsFolder);
		foreach (var src in attachmentSourcePaths)
		{
			if (string.IsNullOrWhiteSpace(src) || !File.Exists(src)) continue;
			var dest = Path.Combine(attachmentsFolder, Path.GetFileName(src));
			File.Copy(src, dest, overwrite: true);
			report.AttachmentPaths.Add(dest);
		}

		var items = (await GetAllAsync(cancellationToken)).ToList();
		items.Add(report);
		await using var stream = File.Create(_dbFilePath);
		await JsonSerializer.SerializeAsync(stream, items, _jsonOptions, cancellationToken);
		return report;
	}
}


