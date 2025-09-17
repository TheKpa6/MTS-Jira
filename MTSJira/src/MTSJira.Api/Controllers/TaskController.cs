using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTSJira.Api.Models;
using MTSJira.Application.Models.Task;
using MTSJira.Application.Services.TaskService.Contract;
using MTSJira.Domain.Common.Enums;
using MTSJira.Domain.Entities.Enums;

namespace MTSJira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult<IEnumerable<TaskDto>>>> GetTasks()
        {
            var result = await _taskService.GetAllTasksAsync();

            switch (result.ErrorCode)
            {
                case CommonErrorCode.None:
                    return Ok(new ApiResult<IEnumerable<TaskDto>>(result.Data));
                case CommonErrorCode.Exception:
                    return BadRequest(new ApiResultNoData(result.Message, result.Ex.StackTrace));
                default:
                    return BadRequest(new ApiResultNoData(result.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<TaskDto>>> GetTask(int id)
        {
            var result = await _taskService.GetTaskByIdAsync(id);

            switch (result.ErrorCode)
            {
                case CommonErrorCode.None:
                    return Ok(new ApiResult<TaskDto>(result.Data));
                case CommonErrorCode.Exception:
                    return BadRequest(new ApiResultNoData(result.Message, result.Ex.StackTrace));
                default:
                    return BadRequest(new ApiResultNoData(result.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult<int>>> CreateTask([FromBody] Models.Task.CreateTaskRequest request)
        {
            var author = User.FindFirst("Login")?.Value ?? string.Empty;
            var requestDto = new CreateTaskRequest
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
            };

            var result = await _taskService.AddTaskAsync(requestDto);

            switch (result.ErrorCode)
            {
                case CommonErrorCode.None:
                    return StatusCode(StatusCodes.Status201Created, new ApiResult<int>(result.Data));
                case CommonErrorCode.Exception:
                    return BadRequest(new ApiResultNoData(result.Message, result.Ex.StackTrace));
                default:
                    return BadRequest(new ApiResultNoData(result.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResult<TaskDto>>> UpdateTask([FromRoute] int id, [FromBody] Models.Task.UpdateTaskRequest request)
        {
            var requestDto = new UpdateTaskRequest
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
            };

            var result = await _taskService.UpdateTaskAsync(id, requestDto);

            switch (result.ErrorCode)
            {
                case CommonErrorCode.None:
                    return Ok(new ApiResult<TaskDto>(result.Data));
                case CommonErrorCode.Exception:
                    return BadRequest(new ApiResultNoData(result.Message, result.Ex.StackTrace));
                default:
                    return BadRequest(new ApiResultNoData(result.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ApiResultNoData))]
        public async Task<ActionResult<ApiResultNoData>> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);

            switch (result.ErrorCode)
            {
                case CommonErrorCode.None:
                    return StatusCode(StatusCodes.Status204NoContent, new ApiResultNoData());
                case CommonErrorCode.Exception:
                    return BadRequest(new ApiResultNoData(result.Message, result.Ex.StackTrace));
                default:
                    return BadRequest(new ApiResultNoData(result.Message));
            }
        }
    }
}
