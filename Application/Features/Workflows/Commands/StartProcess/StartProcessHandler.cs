using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Commands.StartProcess
{
    public class StartProcessHandler : IRequestHandler<StartProcessCommand, StartProcessResponse>
    {
        private readonly ApplicationDbContext _context;

        public StartProcessHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StartProcessResponse> Handle(StartProcessCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = await _context.Workflows
                    .Include(w => w.Steps.OrderBy(s => s.Order))
                    .FirstOrDefaultAsync(w => w.Id == request.WorkflowId && w.IsActive, cancellationToken);

                if (workflow == null)
                {
                    return new StartProcessResponse
                    {
                        Success = false,
                        Message = "Workflow not found or inactive",
                        Errors = new List<string> { "Invalid workflow" }
                    };
                }

                var firstStep = workflow.Steps.OrderBy(s => s.Order).FirstOrDefault();
                if (firstStep == null)
                {
                    return new StartProcessResponse
                    {
                        Success = false,
                        Message = "Workflow has no steps",
                        Errors = new List<string> { "Invalid workflow configuration" }
                    };
                }

                var process = new Process
                {
                    WorkflowId = request.WorkflowId,
                    InitiatorId = request.Initiator, // user ID
                    Status = ProcessStatus.Active,
                    CurrentStep = firstStep.StepName,
                    StartedAt = DateTime.UtcNow
                };

                _context.Processes.Add(process);
                await _context.SaveChangesAsync(cancellationToken);

                return new StartProcessResponse
                {
                    Success = true,
                    Message = "Process started successfully",
                    ProcessId = process.Id,
                    CurrentStep = process.CurrentStep
                };
            }
            catch (Exception ex)
            {
                return new StartProcessResponse
                {
                    Success = false,
                    Message = "Failed to start process",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
