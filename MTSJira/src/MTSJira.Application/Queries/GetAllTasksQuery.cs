using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;
using MTSJira.Application.Models.Task.Enums;

namespace MTSJira.Application.Queries
{
    public class GetAllTasksQuery : IRequest<ICollection<TaskDto>>
    {
    }

    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, ICollection<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<ICollection<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasksData = await _taskRepository.GetAllTasksAsync();

            return tasksData
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                ParentTaskId = t.ParentTaskId,
                Assignee = t.Assignee,
                Author = t.Author,
                Priority = Enum.Parse<TaskPriority>(t.Priority.ToString()),
                Status = Enum.Parse<Models.Task.Enums.TaskStatus>(t.Status.ToString()),
                SubtasksIds = t.Subtasks.Select(t => t.Id).ToList(),
                RelatedTasksIds = t.RelatedTasks.Select(t => new TaskRelationshipDto
                {
                    RelatedTaskId = t.RelatedTaskId,
                    SourceTaskId = t.SourceTaskId,
                }).ToList(),
                RelatedToTasksIds = t.RelatedToTasks.Select(t => new TaskRelationshipDto
                {
                    RelatedTaskId = t.RelatedTaskId,
                    SourceTaskId = t.SourceTaskId,
                }).ToList(),
            })
            .ToList();
        }
    }
}
