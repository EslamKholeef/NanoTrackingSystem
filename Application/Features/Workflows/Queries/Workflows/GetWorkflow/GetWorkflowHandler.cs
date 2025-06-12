using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Queries.Workflows.GetWorkflow
{
    public class GetWorkflowHandler : IRequestHandler<GetWorkflowQuery, GetWorkflowResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetWorkflowHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetWorkflowResponse> Handle(GetWorkflowQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = await _context.Workflows
                    .Include(w => w.Steps.OrderBy(s => s.Order))
                    .Include(w => w.Processes.OrderByDescending(p => p.StartedAt))
                    .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

                if (workflow == null)
                {
                    return new GetWorkflowResponse
                    {
                        Success = false,
                        Message = "Workflow not found"
                    };
                }

                var workflowDto = new WorkflowDetailDto
                {
                    Id = workflow.Id,
                    Name = workflow.Name,
                    Description = workflow.Description,
                    IsActive = workflow.IsActive,
                    CreatedAt = workflow.CreatedAt,
                    UpdatedAt = workflow.UpdatedAt,
                    Steps = workflow.Steps.OrderBy(s => s.Order).Select(s => new WorkflowStepDetailDto
                    {
                        Id = s.Id,
                        StepName = s.StepName,
                        AssignedRole = s.AssignedRole,
                        ActionType = s.ActionType.ToString(),
                        NextStep = s.NextStep,
                        Order = s.Order,
                        RequiresValidation = s.RequiresValidation,
                        ValidationEndpoint = s.ValidationEndpoint
                    }).ToList(),
                    Processes = workflow.Processes.OrderByDescending(p => p.StartedAt).Take(10).Select(p => new ProcessSummaryDto
                    {
                        Id = p.Id,
                        InitiatorId = p.InitiatorId,
                        Status = p.Status.ToString(),
                        CurrentStep = p.CurrentStep,
                        StartedAt = p.StartedAt,
                        CompletedAt = p.CompletedAt
                    }).ToList()
                };

                return new GetWorkflowResponse
                {
                    Success = true,
                    Message = "Workflow retrieved successfully",
                    Workflow = workflowDto
                };
            }
            catch (Exception ex)
            {
                return new GetWorkflowResponse
                {
                    Success = false,
                    Message = "Failed to retrieve workflow"
                };
            }
        }
    }
}
