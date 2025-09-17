using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTSJira.Api.Models;
using MTSJira.Application.Models.Task;
using MTSJira.Application.Services.TaskService.Contract;
using MTSJira.Domain.Common.Enums;

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

        /// <summary>
        /// Метод для поулчения списка всех задач
        /// </summary>
        /// <returns>Список всех задач</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<IEnumerable<TaskDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResultNoData))]
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

        /// <summary>
        /// Метод для получения задачи по её идентификатору
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns>Модель задачи</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<TaskDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResultNoData))]
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

        /// <summary>
        /// Метод создания новой задачи
        /// </summary>
        /// <param name="request">Модель для создания задачи</param>
        /// <returns>Идентификатор новой задачи</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResult<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResultNoData))]
        public async Task<ActionResult<ApiResult<int>>> CreateTask([FromBody] Models.Task.CreateTaskRequest request)
        {
            var author = User.FindFirst("Login")?.Value ?? string.Empty;
            var requestDto = new CreateTaskRequest
            {
                Assignee = request.Assignee,
                ParentTaskId = request.ParentTaskId,
                Title = request.Title,
                Priority = Enum.Parse<Application.Models.Task.Enums.TaskPriority>(request.Priority.ToString()),
                Status = Enum.Parse<Application.Models.Task.Enums.TaskStatus>(request.Status.ToString()),
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

        /// <summary>
        /// Метод обновления задачи со всеми полями
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="request">Модель запроса на обновление задачи</param>
        /// <returns>Модель изменённой задачи</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResult<TaskDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResultNoData))]
        public async Task<ActionResult<ApiResult<TaskDto>>> UpdateTask([FromRoute] int id, [FromBody] Models.Task.UpdateTaskRequest request)
        {
            var requestDto = new UpdateTaskRequest
            {
                Assignee = request.Assignee,
                ParentTaskId = request.ParentTaskId,
                Title = request.Title,
                Priority = Enum.Parse<Application.Models.Task.Enums.TaskPriority>(request.Priority.ToString()),
                Status = Enum.Parse<Application.Models.Task.Enums.TaskStatus>(request.Status.ToString()),
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

        /// <summary>
        /// Метод удаления задачи
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ApiResultNoData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResultNoData))]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);

            switch (result.ErrorCode)
            {
                case CommonErrorCode.None:
                    return StatusCode(StatusCodes.Status204NoContent);
                case CommonErrorCode.Exception:
                    return BadRequest(new ApiResultNoData(result.Message, result.Ex.StackTrace));
                default:
                    return BadRequest(new ApiResultNoData(result.Message));
            }
        }

        /// <summary>
        /// Метод обновления статуса задачи
        /// </summary>
        /// <param name="id">Идентификатор задачи</param>
        /// <param name="request">Модель с новым статусом задачи</param>
        /// <returns></returns>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ApiResultNoData))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResultNoData))]
        public async Task<IActionResult> UpdateTaskStatus([FromRoute] int id, [FromBody] Models.Task.UpdateTaskStatusRequest request)
        {
            var requestDto = new UpdateTaskStatusRequest
            {
                TaskStatus = Enum.Parse<Application.Models.Task.Enums.TaskStatus>(request.TaskStatus.ToString()),
            };

            var result = await _taskService.UpdateTaskStatusAsync(id, requestDto);

            switch (result.ErrorCode)
            {
                case CommonErrorCode.None:
                    return StatusCode(StatusCodes.Status204NoContent);
                case CommonErrorCode.Exception:
                    return BadRequest(new ApiResultNoData(result.Message, result.Ex.StackTrace));
                default:
                    return BadRequest(new ApiResultNoData(result.Message));
            }
        }
    }
}
