using Domain.Entities.Identity;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProcessExecution
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string StepName { get; set; } = string.Empty;

        [Required]
        public string PerformedById { get; set; } = string.Empty;
        public ApplicationUser PerformedBy { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        public ExecutionStatus Status { get; set; } = ExecutionStatus.Completed;

        [MaxLength(1000)]
        public string? Comments { get; set; }

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int ProcessId { get; set; }
        public Process Process { get; set; } = null!;

        public int WorkflowStepId { get; set; }
        public WorkflowStep WorkflowStep { get; set; } = null!;
    }
}
