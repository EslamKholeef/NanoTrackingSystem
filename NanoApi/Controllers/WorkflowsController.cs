using Application.Features.Workflows.Commands.CreateWorkflow;
using Application.Features.Workflows.Queries.Workflows.GetWorkflow;
using Application.Features.Workflows.Queries.Workflows.GetWorkflows;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NanoApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class WorkflowsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkflowsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkflow([FromBody] CreateWorkflowCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(
                nameof(GetWorkflow),
                new { id = result.WorkflowId },
                result
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflow(int id)
        {
            var query = new GetWorkflowQuery { Id = id };
            var result = await _mediator.Send(query);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkflows(
            [FromQuery] bool? isActive = null,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var query = new GetWorkflowsQuery
            {
                IsActive = isActive,
                SearchTerm = searchTerm,
                Page = page,
                PageSize = Math.Min(pageSize, 20) 
            };

            var result = await _mediator.Send(query);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkflow(int id, [FromBody] object updateData)
        {
            // to do
            return Ok(new { message = $"Update workflow {id} - To be implemented" });
        }
    }
}
