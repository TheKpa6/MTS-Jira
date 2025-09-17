using MTSJira.Application.Models.Task;
using MTSJira.Domain.Entities;

namespace MTSJira.Application.InfrastructureContracts.Repositories
{
    public interface ITaskRepository
    {
        Task<int> AddTaskAsync(CreateTaskRequest request);

        Task<TaskData?> GetTaskByIdAsync(int id);

        Task<ICollection<TaskData>> GetAllTasksAsync();

        Task DeleteTaskAsync(TaskData taskData);

        Task<TaskData?> UpdateTaskAsync(int id, UpdateTaskRequest request);
    }
}
