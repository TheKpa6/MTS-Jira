using MTSJira.Application.Models.Task;
using MTSJira.Domain.Entities;

namespace MTSJira.Application.InfrastructureContracts.Repositories
{
    public interface ITaskRepository
    {
        Task AddTaskAsync(TaskData taskData);

        Task<TaskData?> GetTaskByIdAsync(int id);

        Task<ICollection<TaskData>> GetAllTasksAsync();

        Task DeleteTaskAsync(TaskData taskData);

        Task UpdateTaskAsync(TaskData taskData);

        Task<TaskData?> Update(int id, UpdateTaskRequest request);
    }
}
