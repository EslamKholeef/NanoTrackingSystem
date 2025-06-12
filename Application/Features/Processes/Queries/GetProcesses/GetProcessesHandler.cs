using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Processes.Queries.GetProcesses
{
    public class GetProcessesHandler : IRequestHandler<GetProcessesQuery, GetProcessesResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetProcessesHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetProcessesResponse> Handle(GetProcessesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Processes
                .Include(p => p.Workflow)
                .Include(p => p.Executions)
                .AsQueryable();

            if (request.WorkflowId.HasValue)
            {
                query = query.Where(p => p.WorkflowId == request.WorkflowId.Value);
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                if (Enum.TryParse<ProcessStatus>(request.Status, true, out var status))
                {
                    query = query.Where(p => p.Status == status);
                }
            }

            if (!string.IsNullOrEmpty(request.AssignedTo))
            {
                query = query.Where(p => p.CurrentAssigneeId == request.AssignedTo);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var processes = await query
                .OrderByDescending(p => p.StartedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProcessDto
                {
                    Id = p.Id,
                    WorkflowId = p.WorkflowId,
                    WorkflowName = p.Workflow.Name,
                    InitiatorId = p.InitiatorId,
                    Status = p.Status.ToString(),
                    CurrentStep = p.CurrentStep,
                    StartedAt = p.StartedAt,
                    CompletedAt = p.CompletedAt,
                    Executions = p.Executions.Select(e => new ProcessExecutionDto
                    {
                        StepName = e.StepName,
                        PerformedBy = e.PerformedById,
                        Action = e.Action,
                        ExecutedAt = e.ExecutedAt,
                        Comments = e.Comments
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return new GetProcessesResponse
            {
                Success = true,
                Processes = processes,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
