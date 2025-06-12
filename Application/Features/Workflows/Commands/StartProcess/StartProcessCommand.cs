using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Commands.StartProcess
{
    public class StartProcessCommand : IRequest<StartProcessResponse>
    {
        public int WorkflowId { get; set; }
        public string Initiator { get; set; } = string.Empty;
    }

    public class StartProcessResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? ProcessId { get; set; }
        public string? CurrentStep { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
