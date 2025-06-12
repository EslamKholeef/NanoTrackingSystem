using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Workflows.Queries.Workflows.GetWorkflows
{
    public class GetWorkflowsHandler : IRequestHandler<GetWorkflowsQuery, GetWorkflowsResponse>
    {
        private readonly ApplicationDbContext _context;

        public GetWorkflowsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetWorkflowsResponse> Handle(GetWorkflowsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _context.Workflows
                    .Include(w => w.Steps.OrderBy(s => s.Order))
                    .AsQueryable();

                if (request.IsActive.HasValue)
                {
                    query = query.Where(w => w.IsActive == request.IsActive.Value);
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(w => w.Name.Contains(request.SearchTerm) ||
                                           (w.Description != null && w.Description.Contains(request.SearchTerm)));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var workflows = await query
                    .OrderByDescending(w => w.CreatedAt)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(w => new WorkflowDto
                    {
                        Id = w.Id,
                        Name = w.Name,
                        Description = w.Description,
                        IsActive = w.IsActive,
                        CreatedAt = w.CreatedAt,
                        UpdatedAt = w.UpdatedAt,
                        StepsCount = w.Steps.Count,
                        Steps = w.Steps.OrderBy(s => s.Order).Select(s => new WorkflowStepDto
                        {
                            Id = s.Id,
                            StepName = s.StepName,
                            AssignedRole = s.AssignedRole,
                            ActionType = s.ActionType.ToString(),
                            NextStep = s.NextStep,
                            Order = s.Order,
                            RequiresValidation = s.RequiresValidation,
                            ValidationEndpoint = s.ValidationEndpoint
                        }).ToList()
                    })
                    .ToListAsync(cancellationToken);

                return new GetWorkflowsResponse
                {
                    Success = true,
                    Message = "Workflows retrieved successfully",
                    Workflows = workflows,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize
                };
            }
            catch (Exception ex)
            {
                return new GetWorkflowsResponse
                {
                    Success = false,
                    Message = "Failed to retrieve workflows",
                    Workflows = new List<WorkflowDto>()
                };
            }
        }
    }
}
