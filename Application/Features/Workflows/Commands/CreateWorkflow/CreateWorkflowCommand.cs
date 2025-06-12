using MediatR;
using Shared.DTOs.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Commands.CreateWorkflow
{
    public class CreateWorkflowCommand : IRequest<CreateWorkflowResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<CreateWorkflowStepDto> Steps { get; set; } = new();
    }

    public class CreateWorkflowResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? WorkflowId { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
