using MTSJira.Domain.Entities;

namespace MTSJira.Application.InfrastructureContracts.Repositories
{
    public interface ITaskRepository
    {
        Task AddTaskAsync(TaskData taskData);

        Task<TaskData> GetTaskByIdAsync(int id);

        Task<IEnumerable<TaskData>> GetAllTasksAsync();

        Task DeleteTaskAsync(TaskData taskData);

        Task UpdateTaskAsync(TaskData taskData);
    }
}
