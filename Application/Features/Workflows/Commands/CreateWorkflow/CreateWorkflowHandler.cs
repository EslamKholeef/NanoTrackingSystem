using Domain.Entities;
using Domain.Enums;
using MediatR;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Commands.CreateWorkflow
{
    public class CreateWorkflowHandler : IRequestHandler<CreateWorkflowCommand, CreateWorkflowResponse>
    {
        private readonly ApplicationDbContext _context;

        public CreateWorkflowHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateWorkflowResponse> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = new Workflow
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Workflows.Add(workflow);
                await _context.SaveChangesAsync(cancellationToken);

                for (int i = 0; i < request.Steps.Count; i++)
                {
                    var stepDto = request.Steps[i];

                    var step = new WorkflowStep
                    {
                        WorkflowId = workflow.Id,
                        StepName = stepDto.StepName,
                        AssignedRole = stepDto.AssignedTo,
                        ActionType = MapActionType(stepDto.ActionType),
                        NextStep = stepDto.NextStep,
                        Order = i + 1,
                        RequiresValidation = stepDto.RequiresValidation,
                        ValidationEndpoint = stepDto.ValidationEndpoint
                    };

                    _context.WorkflowSteps.Add(step);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new CreateWorkflowResponse
                {
                    Success = true,
                    Message = "Workflow created successfully",
                    WorkflowId = workflow.Id
                };
            }
            catch (Exception ex)
            {
                return new CreateWorkflowResponse
                {
                    Success = false,
                    Message = "Failed to create workflow",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        private static ActionType MapActionType(string actionType)
        {
            return actionType.ToLower() switch
            {
                "input" => ActionType.Input,
                "approve_reject" => ActionType.ApproveReject,
                "review" => ActionType.Review,
                "complete" => ActionType.Complete,
                _ => ActionType.Input
            };
        }
    }
}
