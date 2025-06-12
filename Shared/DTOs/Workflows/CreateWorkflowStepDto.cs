using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Workflows
{
    public class CreateWorkflowStepDto
    {
        public string StepName { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty; // The Role name
        public string ActionType { get; set; } = string.Empty;
        public string? NextStep { get; set; }
        public bool RequiresValidation { get; set; } = false;
        public string? ValidationEndpoint { get; set; }
    }
}
