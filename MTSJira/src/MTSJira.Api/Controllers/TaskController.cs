using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTSJira.Api.Models.Task;
using MTSJira.Application.Commands;
using MTSJira.Application.Models.Task;
using MTSJira.Application.Queries;
using System.Security.Claims;

namespace MTSJira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            var query = new GetAllTasksQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var query = new GetTaskByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateTask([FromBody] Models.Task.CreateTaskRequest request)
        {
            var author = User.FindFirst("Login")?.Value ?? string.Empty;
            var command = new CreateTaskCommand
            {
                Request = new Application.Models.Task.CreateTaskRequest
                {
                    Assignee = request.Assignee,
                    ParentTaskId = request.ParentTaskId,
                    Title = request.Title,
                    Priority = Enum.Parse<Domain.Enums.TaskPriority>(request.Priority.ToString()),
                    Status = Enum.Parse<Domain.Enums.TaskStatus>(request.Status.ToString()),
                },
                Author = author
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskDto>> UpdateTask([FromRoute] int id, [FromBody] Models.Task.UpdateTaskRequest request)
        {
            var command = new UpdateTaskCommand
            {
                Id = id,
                Request = new Application.Models.Task.UpdateTaskRequest
                {
                    Assignee = request.Assignee,
                    Author = request.Author,
                    ParentTaskId = request.ParentTaskId,
                    Title = request.Title,
                    Priority = Enum.Parse<Domain.Enums.TaskPriority>(request.Priority.ToString()),
                    Status = Enum.Parse<Domain.Enums.TaskStatus>(request.Status.ToString()),
                },
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var command = new DeleteTaskCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
