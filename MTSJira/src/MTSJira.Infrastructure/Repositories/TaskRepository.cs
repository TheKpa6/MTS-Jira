using Microsoft.EntityFrameworkCore;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;
using MTSJira.Domain.Common.Enums;
using MTSJira.Domain.Entities;
using MTSJira.Domain.Exceptions;
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

        public async Task<int> AddTaskAsync(CreateTaskRequest request)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var task = new TaskData
                {
                    Title = request.Title,
                    Status = request.Status,
                    Priority = request.Priority,
                    Author = request.Author,
                    Assignee = request.Assignee,
                };

                if (request.ParentTaskId.HasValue)
                {
                    await SetParentTask(task, request.ParentTaskId.Value);
                }

                await _dbContext.Tasks.AddAsync(task);
                await _dbContext.SaveChangesAsync();

                await AddTaskRelationships(task, request);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return task.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteTaskAsync(TaskData taskData)
        {
            _dbContext.TaskRelationships.RemoveRange(taskData.RelatedTasks);
            _dbContext.TaskRelationships.RemoveRange(taskData.RelatedToTasks);
            _dbContext.Tasks.Remove(taskData);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<TaskData>> GetAllTasksAsync()
        {
            return await _dbContext.Tasks
                .Include(t => t.Subtasks)
                .Include(t => t.RelatedTasks)
                .Include(t => t.RelatedToTasks)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<TaskData?> GetTaskByIdAsync(int id)
        {
            return await _dbContext.Tasks
                .Include(t => t.Subtasks)
                .Include(t => t.RelatedTasks)
                .Include(t => t.RelatedToTasks)
                .AsSplitQuery()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskData?> UpdateTaskAsync(int id, UpdateTaskRequest request)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var task = await GetTaskByIdAsync(id);

                if (task == null)
                    throw new JiraApplicationException($"Task with ID {id} not found", CommonErrorCode.ObjectNotFound);

                UpdateTaskProperties(task, request);

                await UpdateParentTask(task, request.ParentTaskId);

                await UpdateTaskRelationships(task, request);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetTaskByIdAsync(id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private void UpdateTaskProperties(TaskData task, UpdateTaskRequest request)
        {
            task.Title = request.Title;
            task.Status = request.Status;
            task.Priority = request.Priority;
            task.Author = request.Author;
            task.Assignee = request.Assignee;
        }

        private async Task UpdateParentTask(TaskData task, int? newParentTaskId)
        {
            if (task.ParentTaskId == newParentTaskId)
                return;

            if (newParentTaskId.HasValue)
            {
                var parentTask = await _dbContext.Tasks
                    .FirstOrDefaultAsync(t => t.Id == newParentTaskId.Value);

                if (parentTask == null)
                    throw new JiraApplicationException($"Parent task with ID {newParentTaskId} not found", CommonErrorCode.ObjectNotFound);

                if (await IsCycle(task.Id, newParentTaskId.Value))
                    throw new JiraApplicationException("Cannot set parent task: circular dependency detected", CommonErrorCode.InvalidArgument);
            }

            task.ParentTaskId = newParentTaskId;
        }

        private async Task<bool> IsCycle(int taskId, int? potentialParentId)
        {
            var current = potentialParentId;
            var visited = new HashSet<int> { taskId };

            while (current.HasValue)
            {
                if (visited.Contains(current.Value))
                    return true;

                visited.Add(current.Value);

                var parentTask = await _dbContext.Tasks
                    .FirstOrDefaultAsync(t => t.Id == current.Value);

                current = parentTask?.ParentTaskId;
            }

            return false;
        }

        private async Task UpdateTaskRelationships(TaskData task, UpdateTaskRequest request)
        {
            _dbContext.TaskRelationships.RemoveRange(task.RelatedTasks);
            _dbContext.TaskRelationships.RemoveRange(task.RelatedToTasks);

            foreach (var relatedTaskDto in request.RelatedTasks)
            {
                await ValidateAndAddRelationship(
                    task.Id,
                    relatedTaskDto.RelatedTaskId,
                    true);
            }

            foreach (var incomingRelationDto in request.RelatedToTasks)
            {
                await ValidateAndAddRelationship(
                    task.Id,
                    incomingRelationDto.SourceTaskId,
                    false);
            }
        }

        private async Task ValidateAndAddRelationship(int taskId, int relatedTaskId, bool isRelatedTask)
        {
            var relatedTask = await _dbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == relatedTaskId);

            if (relatedTask == null)
                throw new JiraApplicationException($"Related task with ID {relatedTaskId} not found", CommonErrorCode.ObjectNotFound);

            if (relatedTaskId == taskId)
                throw new JiraApplicationException("Cannot create relationship to self", CommonErrorCode.InvalidArgument);

            var relationship = new TaskRelationshipData
            {
                SourceTaskId = isRelatedTask ? taskId : relatedTaskId,
                RelatedTaskId = isRelatedTask ? relatedTaskId : taskId,
            };

            await _dbContext.TaskRelationships.AddAsync(relationship);
        }

        private async Task SetParentTask(TaskData task, int parentTaskId)
        {
            var parentTask = await _dbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == parentTaskId);

            if (parentTask == null)
                throw new JiraApplicationException($"Parent task with ID {parentTaskId} not found", CommonErrorCode.ObjectNotFound);

            task.ParentTaskId = parentTaskId;
        }

        private async Task AddTaskRelationships(TaskData task, CreateTaskRequest request)
        {
            foreach (var relatedTaskDto in request.RelatedTasks)
            {
                await ValidateAndAddRelationship(
                    task.Id,
                    relatedTaskDto.RelatedTaskId,
                    true);
            }

            foreach (var incomingRelationDto in request.RelatedToTasks)
            {
                await ValidateAndAddRelationship(
                    task.Id,
                    incomingRelationDto.SourceTaskId,
                    false);
            }
        }
    }
}
