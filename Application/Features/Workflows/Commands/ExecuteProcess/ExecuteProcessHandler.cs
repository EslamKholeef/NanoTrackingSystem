using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Features.Workflows.Commands.ExecuteProcess
{
    public class ExecuteProcessHandler : IRequestHandler<ExecuteProcessCommand, ExecuteProcessResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidationService _validationService;

        public ExecuteProcessHandler(ApplicationDbContext context, IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        public async Task<ExecuteProcessResponse> Handle(ExecuteProcessCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var process = await _context.Processes
                    .Include(p => p.Workflow)
                        .ThenInclude(w => w.Steps.OrderBy(s => s.Order))
                    .FirstOrDefaultAsync(p => p.Id == request.ProcessId, cancellationToken);

                if (process == null)
                {
                    return new ExecuteProcessResponse
                    {
                        Success = false,
                        Message = "Process not found",
                        Errors = new List<string> { "Invalid process ID" }
                    };
                }

                // Validate process is active
                if (process.Status != ProcessStatus.Active)
                {
                    return new ExecuteProcessResponse
                    {
                        Success = false,
                        Message = "Process is not active",
                        Errors = new List<string> { $"Process status is {process.Status}" }
                    };
                }

                var currentStep = process.Workflow.Steps
                    .FirstOrDefault(s => s.StepName == request.StepName);

                if (currentStep == null)
                {
                    return new ExecuteProcessResponse
                    {
                        Success = false,
                        Message = "Step not found in workflow",
                        Errors = new List<string> { "Invalid step name" }
                    };
                }

                // Validate it's the current step
                if (process.CurrentStep != request.StepName)
                {
                    return new ExecuteProcessResponse
                    {
                        Success = false,
                        Message = "Cannot execute this step",
                        Errors = new List<string> { $"Current step is {process.CurrentStep}" }
                    };
                }

                // **VALIDATION **
                if (currentStep.RequiresValidation)
                {
                    var validationResult = await _validationService.ValidateStepAsync(
                        currentStep.ValidationEndpoint!,
                        request.ProcessId,
                        request.StepName,
                        request.Action,
                        cancellationToken);

                    // Log validation attempt
                    var validationLog = new ProcessValidationLog
                    {
                        ProcessId = request.ProcessId,
                        StepName = request.StepName,
                        ValidationEndpoint = currentStep.ValidationEndpoint,
                        ValidationResult = validationResult.IsValid ? StepValidationResult.Success : StepValidationResult.Failed,
                        ValidationResponse = validationResult.Response,
                        ErrorMessage = validationResult.ErrorMessage,
                        ValidatedAt = DateTime.UtcNow
                    };

                    _context.ProcessValidationLogs.Add(validationLog);

                    if (!validationResult.IsValid)
                    {
                        await _context.SaveChangesAsync(cancellationToken);

                        return new ExecuteProcessResponse
                        {
                            Success = false,
                            Message = "Validation failed",
                            ProcessStatus = process.Status.ToString(),
                            ValidationErrors = new List<string> { validationResult.ErrorMessage ?? "External validation failed" }
                        };
                    }
                }

                // Execute the step
                var execution = new ProcessExecution
                {
                    ProcessId = request.ProcessId,
                    WorkflowStepId = currentStep.Id,
                    StepName = request.StepName,
                    PerformedById = request.PerformedBy,
                    Action = request.Action,
                    Comments = request.Comments,
                    Status = ExecutionStatus.Completed,
                    ExecutedAt = DateTime.UtcNow
                };

                _context.ProcessExecutions.Add(execution);

                var nextStepResult = DetermineNextStep(process, currentStep, request.Action);

                process.CurrentStep = nextStepResult.NextStep;
                process.Status = nextStepResult.ProcessStatus;

                if (nextStepResult.ProcessStatus == ProcessStatus.Completed)
                {
                    process.CompletedAt = DateTime.UtcNow;
                }

                _context.Processes.Update(process);
                await _context.SaveChangesAsync(cancellationToken);

                return new ExecuteProcessResponse
                {
                    Success = true,
                    Message = "Step executed successfully",
                    NextStep = nextStepResult.NextStep,
                    ProcessStatus = nextStepResult.ProcessStatus.ToString()
                };
            }
            catch (Exception ex)
            {
                return new ExecuteProcessResponse
                {
                    Success = false,
                    Message = "Failed to execute step",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        private static (string? NextStep, ProcessStatus ProcessStatus) DetermineNextStep(
            Process process, WorkflowStep currentStep, string action)
        {
            //  rejection
            if (action.ToLower() == "reject")
            {
                return (null, ProcessStatus.Rejected);
            }

            //  approval or completion
            if (string.IsNullOrEmpty(currentStep.NextStep) || currentStep.NextStep == "Completed")
            {
                return (null, ProcessStatus.Completed);
            }

            // Move to next step
            return (currentStep.NextStep, ProcessStatus.Active);
        }
    }
}
