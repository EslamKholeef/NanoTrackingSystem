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
    public class Process
    {
        public int Id { get; set; }

        [Required]
        public string InitiatorId { get; set; } = string.Empty;
        public ApplicationUser Initiator { get; set; } = null!;

        public ProcessStatus Status { get; set; } = ProcessStatus.Active;

        [MaxLength(200)]
        public string? CurrentStep { get; set; }

        //This is Current assignee (who should act next) 
        public string? CurrentAssigneeId { get; set; }
        public ApplicationUser? CurrentAssignee { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        // Foreign Key
        public int WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;

        // Navigation Properties
        public ICollection<ProcessExecution> Executions { get; set; } = new List<ProcessExecution>();
        public ICollection<ProcessValidationLog> ValidationLogs { get; set; } = new List<ProcessValidationLog>();
    }
}
