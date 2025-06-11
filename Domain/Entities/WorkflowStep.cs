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
    public class WorkflowStep
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string StepName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string AssignedRole { get; set; } = string.Empty; // IdentityRole name

        // Optional: Assign to specific user instead of role
        public string? AssignedUserId { get; set; }
        public ApplicationUser? AssignedUser { get; set; }

        public ActionType ActionType { get; set; }

        [MaxLength(200)]
        public string? NextStep { get; set; }

        public int Order { get; set; }
        public bool RequiresValidation { get; set; } = false;

        [MaxLength(500)]
        public string? ValidationEndpoint { get; set; }

        // Foreign Key
        public int WorkflowId { get; set; }
        public Workflow Workflow { get; set; } = null!;

        // Navigation Properties
        public ICollection<ProcessExecution> ProcessExecutions { get; set; } = new List<ProcessExecution>();
    }
}
