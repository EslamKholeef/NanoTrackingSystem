using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Processes.Queries.GetProcesses
{
    public class GetProcessesQuery : IRequest<GetProcessesResponse>
    {
        public int? WorkflowId { get; set; }
        public string? Status { get; set; }
        public string? AssignedTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetProcessesResponse
    {
        public bool Success { get; set; }
        public List<ProcessDto> Processes { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ProcessDto
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; } = string.Empty;
        public string InitiatorId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? CurrentStep { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<ProcessExecutionDto> Executions { get; set; } = new();
    }

    public class ProcessExecutionDto
    {
        public string StepName { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; }
        public string? Comments { get; set; }
    }
}
