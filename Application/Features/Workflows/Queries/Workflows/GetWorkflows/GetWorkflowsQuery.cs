using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Queries.Workflows.GetWorkflows
{
    public class GetWorkflowsQuery : IRequest<GetWorkflowsResponse>
    {
        public bool? IsActive { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    public class GetWorkflowsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<WorkflowDto> Workflows { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class WorkflowDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int StepsCount { get; set; }
        public List<WorkflowStepDto> Steps { get; set; } = new();
    }

    public class WorkflowStepDto
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
}
