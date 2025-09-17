using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTSJira.Application.Commands;
using MTSJira.Application.Models.Task;
using MTSJira.Application.Queries;
using MTSJira.Domain.Entities.Enums;

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
                Request = new CreateTaskRequest
                {
                    Assignee = request.Assignee,
                    ParentTaskId = request.ParentTaskId,
                    Title = request.Title,
                    Priority = Enum.Parse<TaskPriority>(request.Priority.ToString()),
                    Status = Enum.Parse<Domain.Entities.Enums.TaskStatus>(request.Status.ToString()),
                    Author = author,
                    RelatedTasks = request.RelatedTasks.Select(t => new RelatedTaskDto
                    {
                        RelatedTaskId = t.RelatedTaskId
                    }).ToList(),
                    RelatedToTasks = request.RelatedToTasks.Select(t => new RelatedToTasksDto
                    {
                        SourceTaskId = t.SourceTaskId,
                    }).ToList()
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
                Request = new UpdateTaskRequest
                {
                    Assignee = request.Assignee,
                    ParentTaskId = request.ParentTaskId,
                    Title = request.Title,
                    Priority = Enum.Parse<TaskPriority>(request.Priority.ToString()),
                    Status = Enum.Parse<Domain.Entities.Enums.TaskStatus>(request.Status.ToString()),
                    Author = request.Author,
                    RelatedTasks = request.RelatedTasks.Select(t => new RelatedTaskDto
                    {
                        RelatedTaskId = t.RelatedTaskId
                    }).ToList(),
                    RelatedToTasks = request.RelatedToTasks.Select(t => new RelatedToTasksDto
                    {
                        SourceTaskId = t.SourceTaskId,
                    }).ToList()
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
