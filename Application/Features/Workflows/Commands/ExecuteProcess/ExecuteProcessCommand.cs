using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Commands.ExecuteProcess
{
    public class ExecuteProcessCommand : IRequest<ExecuteProcessResponse>
    {
        public int ProcessId { get; set; }
        public string StepName { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? Comments { get; set; }
    }

    public class ExecuteProcessResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? NextStep { get; set; }
        public string ProcessStatus { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public List<string> ValidationErrors { get; set; } = new();
    }
}
