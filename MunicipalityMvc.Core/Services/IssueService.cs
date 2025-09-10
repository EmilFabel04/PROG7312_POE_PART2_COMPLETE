using System.Text.Json;
using System.Linq;
using MunicipalityMvc.Core.Models;

namespace MunicipalityMvc.Core.Services;

/// <summary>
/// Stores issue reports using a FIFO queue and persists them to a JSON file.
/// This keeps Part 1 simple, offline-friendly, and easy to review.
/// </summary>
public sealed class IssueService : IIssueService
{
	private readonly string _dataDirectory;
	private readonly string _dbFilePath;
	private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
	private readonly object _sync = new();
	private readonly Queue<IssueReport> _queue = new();

	// Create the service, prepare the data folder, and load any previously saved issues.
	public IssueService(string baseDataDirectory)
	{
		_dataDirectory = Path.Combine(baseDataDirectory, "data");
		Directory.CreateDirectory(_dataDirectory);
		_dbFilePath = Path.Combine(_dataDirectory, "issues.json");
		if (!File.Exists(_dbFilePath)) File.WriteAllText(_dbFilePath, "[]");

		// Load persisted items into the in-memory queue preserving order
		using var readStream = File.OpenRead(_dbFilePath);
		var existing = JsonSerializer.Deserialize<List<IssueReport>>(readStream, _jsonOptions) ?? new();
		foreach (var item in existing)
		{
			_queue.Enqueue(item);
		}
	}

	/// <inheritdoc />
	// Return a snapshot of the current queue as a read-only list.
	public Task<IReadOnlyList<IssueReport>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		lock (_sync)
		{
			return Task.FromResult((IReadOnlyList<IssueReport>)_queue.ToList());
		}
	}

	/// <inheritdoc />
	// Enqueue a new report, copy attachments, then persist the full queue to disk.
	public async Task<IssueReport> AddAsync(IssueReport report, IEnumerable<string> attachmentSourcePaths, CancellationToken cancellationToken = default)
	{
		// Generate a short human-friendly ticket code (e.g., MS-ABC123)
		report.TicketCode = GenerateTicketCode();
		var attachmentsFolder = Path.Combine(_dataDirectory, report.Id.ToString());
		Directory.CreateDirectory(attachmentsFolder);
		foreach (var src in attachmentSourcePaths)
		{
			if (string.IsNullOrWhiteSpace(src) || !File.Exists(src)) continue;
			var dest = Path.Combine(attachmentsFolder, Path.GetFileName(src));
			File.Copy(src, dest, overwrite: true);
			report.AttachmentPaths.Add(dest);
		}

		lock (_sync)
		{
			_queue.Enqueue(report);
			PersistQueue();
		}

		return await Task.FromResult(report);
	}

	// Generate a short human-friendly ticket code (e.g., MS-ABC123).
	private static string GenerateTicketCode()
	{
		const string alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // avoid similar-looking chars
		var rnd = Random.Shared;
		Span<char> buffer = stackalloc char[6];
		for (var i = 0; i < buffer.Length; i++) buffer[i] = alphabet[rnd.Next(alphabet.Length)];
		return $"MS-{new string(buffer)}";
	}

	/// <inheritdoc />
	// Look up a report by its unique Id.
	public Task<IssueReport?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		lock (_sync)
		{
			return Task.FromResult(_queue.FirstOrDefault(x => x.Id == id));
		}
	}

	/// <inheritdoc />
	// Compute a 1-based position for a report in the queue.
	public Task<int?> GetPositionAsync(Guid id, CancellationToken cancellationToken = default)
	{
		lock (_sync)
		{
			var list = _queue.ToList();
			var idx = list.FindIndex(x => x.Id == id);
			return Task.FromResult(idx >= 0 ? (int?)(idx + 1) : null);
		}
	}

	// Save the entire queue to the JSON file (simple and robust for this scope).
	private void PersistQueue()
	{
		using var stream = File.Create(_dbFilePath);
		JsonSerializer.Serialize(stream, _queue.ToList(), _jsonOptions);
	}
}


