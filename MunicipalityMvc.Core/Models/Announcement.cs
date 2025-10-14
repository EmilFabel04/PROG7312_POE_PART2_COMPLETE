using System.ComponentModel.DataAnnotations;

namespace MunicipalityMvc.Core.Models
{
    // announcement model
    public class Announcement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        
        public DateTime? ExpiryDate { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string Priority { get; set; } = "Normal";
        
        public bool IsActive { get; set; } = true;
    }
}
