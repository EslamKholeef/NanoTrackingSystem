using Domain.Enums;

using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class ProcessValidationLog
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string StepName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ValidationEndpoint { get; set; }

        public StepValidationResult ValidationResult { get; set; }

        [MaxLength(2000)]
        public string? ValidationResponse { get; set; }

        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }

        public DateTime ValidatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int ProcessId { get; set; }
        public Process Process { get; set; } = null!;
    }
}
