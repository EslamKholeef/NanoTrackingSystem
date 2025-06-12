using FluentValidation;
using Shared.DTOs.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Commands.CreateWorkflow
{
    public class CreateWorkflowValidator : AbstractValidator<CreateWorkflowCommand>
    {
        public CreateWorkflowValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Workflow name is required")
                .MaximumLength(200).WithMessage("Workflow name must not exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.Steps)
                .NotEmpty().WithMessage("At least one step is required")
                .Must(steps => steps != null && steps.Count > 0).WithMessage("Workflow must have at least one step");

            RuleForEach(x => x.Steps).SetValidator(new WorkflowStepValidator());
        }
    }

    public class WorkflowStepValidator : AbstractValidator<CreateWorkflowStepDto>
    {
        public WorkflowStepValidator()
        {
            RuleFor(x => x.StepName)
                .NotEmpty().WithMessage("Step name is required")
                .MaximumLength(200).WithMessage("Step name must not exceed 200 characters");

            RuleFor(x => x.AssignedTo)
                .NotEmpty().WithMessage("Assigned role is required")
                .MaximumLength(100).WithMessage("Assigned role must not exceed 100 characters");

            RuleFor(x => x.ActionType)
                .NotEmpty().WithMessage("Action type is required")
                .Must(x => x == "input" || x == "approve_reject" || x == "review" || x == "complete")
                .WithMessage("Action type must be: input, approve_reject, review, or complete");
        }
    }
}
