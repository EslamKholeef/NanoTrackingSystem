using Application.Features.Processes.Queries.GetProcesses;
using Application.Features.Workflows.Commands.ExecuteProcess;
using Application.Features.Workflows.Commands.StartProcess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NanoApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize] 
    public class ProcessesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProcessesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartProcess([FromBody] StartProcessCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            command.Initiator = currentUserId;

            var result = await _mediator.Send(command);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost("execute")]
        public async Task<IActionResult> ExecuteProcess([FromBody] ExecuteProcessCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            command.PerformedBy = currentUserId;

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                if (result.ValidationErrors.Any())
                {
                    return BadRequest(new
                    {
                        message = result.Message,
                        errors = result.Errors,
                        validationErrors = result.ValidationErrors
                    });
                }
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetProcesses(
            [FromQuery] int? workflowId = null,
            [FromQuery] string? status = null,
            [FromQuery] string? assignedTo = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetProcessesQuery
            {
                WorkflowId = workflowId,
                Status = status,
                AssignedTo = assignedTo,
                Page = page,
                PageSize = Math.Min(pageSize, 20)
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("my-tasks")]
        public async Task<IActionResult> GetMyTasks(
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var query = new GetProcessesQuery
            {
                Status = status,
                AssignedTo = currentUserId,
                Page = page,
                PageSize = Math.Min(pageSize, 50)
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
