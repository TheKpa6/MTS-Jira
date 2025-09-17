using MediatR;
using Microsoft.Extensions.Logging;
using MTSJira.Application.Commands;
using MTSJira.Application.Handlers;
using MTSJira.Application.Models.Task;
using MTSJira.Application.Queries;
using MTSJira.Application.Services.TaskService.Contract;
using MTSJira.Domain.Exceptions;

namespace MTSJira.Application.Services.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly ILogger<TaskService> _logger;
        private readonly IMediator _mediator;

        public TaskService(ILogger<TaskService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ApplicationCommonServiceHandlerResult<int>> AddTaskAsync(CreateTaskRequest request)
        {
            try
            {
                var command = new CreateTaskCommand
                {
                    Request = request
                };

                var result = await _mediator.Send(command);

                return ApplicationCommonServiceHandlerResult<int>.CreateSuccess(result);
            }
            catch (JiraApplicationException ex)
            {
                _logger.LogError(ex.Message);
                return ApplicationCommonServiceHandlerResult<int>.CreateError(ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Can't add task with name: {Title}", request.Title);
                return ApplicationCommonServiceHandlerResult<int>.CreateException(ex);
            }
        }

        public async Task<ApplicationCommonServiceHandlerResultNoData> DeleteTaskAsync(int id)
        {
            try
            {
                var command = new DeleteTaskCommand
                {
                    Id = id
                };

                var result = await _mediator.Send(command);

                return ApplicationCommonServiceHandlerResultNoData.CreateSuccess();
            }
            catch (JiraApplicationException ex)
            {
                _logger.LogError(ex.Message);
                return ApplicationCommonServiceHandlerResultNoData.CreateError(ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Can't delete task with id: {Id}", id);
                return ApplicationCommonServiceHandlerResultNoData.CreateException(ex);
            }
        }

        public async Task<ApplicationCommonServiceHandlerResult<ICollection<TaskDto>>> GetAllTasksAsync()
        {
            try
            {
                var query = new GetAllTasksQuery();

                var result = await _mediator.Send(query);

                return ApplicationCommonServiceHandlerResult<ICollection<TaskDto>>.CreateSuccess(result);
            }
            catch (JiraApplicationException ex)
            {
                _logger.LogError(ex.Message);
                return ApplicationCommonServiceHandlerResult<ICollection<TaskDto>>.CreateError(ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Can't get all tasks");
                return ApplicationCommonServiceHandlerResult<ICollection<TaskDto>>.CreateException(ex);
            }
        }

        public async Task<ApplicationCommonServiceHandlerResult<TaskDto?>> GetTaskByIdAsync(int id)
        {
            try
            {
                var query = new GetTaskByIdQuery { Id = id };

                var result = await _mediator.Send(query);

                return ApplicationCommonServiceHandlerResult<TaskDto?>.CreateSuccess(result);
            }
            catch (JiraApplicationException ex)
            {
                _logger.LogError(ex.Message);
                return ApplicationCommonServiceHandlerResult<TaskDto?>.CreateError(ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Can't get task with id: {Id}", id);
                return ApplicationCommonServiceHandlerResult<TaskDto?>.CreateException(ex);
            }
        }

        public async Task<ApplicationCommonServiceHandlerResult<TaskDto?>> UpdateTaskAsync(int id, UpdateTaskRequest request)
        {
            try
            {
                var command = new UpdateTaskCommand
                {
                    Request = request,
                    Id = id
                };

                var result = await _mediator.Send(command);

                return ApplicationCommonServiceHandlerResult<TaskDto?>.CreateSuccess(result);
            }
            catch (JiraApplicationException ex)
            {
                _logger.LogError(ex.Message);
                return ApplicationCommonServiceHandlerResult<TaskDto?>.CreateError(ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Can't update task with id: {Id}", id);
                return ApplicationCommonServiceHandlerResult<TaskDto?>.CreateException(ex);
            }
        }
    }
}
