using System.ComponentModel.DataAnnotations;

namespace MunicipalityMvc.Core.Models
{
    // event model for events
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; } // FIX
        
        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
        
        public bool IsRecurring { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
