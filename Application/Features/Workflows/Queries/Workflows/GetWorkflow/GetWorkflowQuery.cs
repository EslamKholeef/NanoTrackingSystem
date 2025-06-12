using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Queries.Workflows.GetWorkflow
{
    public class GetWorkflowQuery : IRequest<GetWorkflowResponse>
    {
        public int Id { get; set; }
    }

    public class GetWorkflowResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public WorkflowDetailDto? Workflow { get; set; }
    }

    public class WorkflowDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<WorkflowStepDetailDto> Steps { get; set; } = new();
        public List<ProcessSummaryDto> Processes { get; set; } = new();
    }

    public class WorkflowStepDetailDto
    {
        public int Id { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string AssignedRole { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string? NextStep { get; set; }
        public int Order { get; set; }
        public bool RequiresValidation { get; set; }
        public string? ValidationEndpoint { get; set; }
    }

    public class ProcessSummaryDto
    {
        public int Id { get; set; }
        public string InitiatorId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? CurrentStep { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
