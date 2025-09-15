using Microsoft.EntityFrameworkCore;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Domain.Entities;
using MTSJira.Infrastructure.Database.Contexts;

namespace MTSJira.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly JiraDbContext _dbContext;

        public TaskRepository(JiraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddTaskAsync(TaskData taskData)
        {
            await _dbContext.Tasks.AddAsync(taskData);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskData taskData)
        {
            _dbContext.Tasks.Update(taskData);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(TaskData taskData)
        {
            _dbContext.Tasks.Remove(taskData);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskData>> GetAllTasksAsync()
        {
            return await _dbContext.Tasks.Include(x => x.ParentTask).Include(x => x.Subtasks).ToListAsync();
        }

        public async Task<TaskData> GetTaskByIdAsync(int id)
        {
            return await _dbContext.Tasks.Include(x => x.ParentTask).Include(x => x.Subtasks).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
