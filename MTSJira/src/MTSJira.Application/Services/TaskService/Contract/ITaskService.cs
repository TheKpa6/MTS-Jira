using MTSJira.Application.Handlers;
using MTSJira.Application.Models.Task;

namespace MTSJira.Application.Services.TaskService.Contract
{
    public interface ITaskService
    {
        Task<ApplicationCommonServiceHandlerResult<int>> AddTaskAsync(CreateTaskRequest request);

        Task<ApplicationCommonServiceHandlerResult<TaskDto?>> GetTaskByIdAsync(int id);

        Task<ApplicationCommonServiceHandlerResult<ICollection<TaskDto>>> GetAllTasksAsync();

        Task<ApplicationCommonServiceHandlerResultNoData> DeleteTaskAsync(int id);

        Task<ApplicationCommonServiceHandlerResult<TaskDto?>> UpdateTaskAsync(int id, UpdateTaskRequest request);
    }
}
