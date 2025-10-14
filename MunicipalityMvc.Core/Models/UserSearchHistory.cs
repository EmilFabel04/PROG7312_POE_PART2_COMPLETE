namespace MunicipalityMvc.Core.Models
{
    // user search history model
    public class UserSearchHistory
    {
        public string SearchTerm { get; set; } = string.Empty;
        public DateTime SearchDate { get; set; } = DateTime.UtcNow;
        public string? Category { get; set; }
    }
}
