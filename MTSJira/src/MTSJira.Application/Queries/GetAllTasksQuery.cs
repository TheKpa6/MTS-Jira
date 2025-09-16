using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;

namespace MTSJira.Application.Queries
{
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
    }

    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var taskData = await _taskRepository.GetAllTasksAsync();

            return taskData
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                ParentTaskId = t.ParentTaskId,
                Assignee = t.Assignee,
                Author = t.Author,
                Priority = t.Priority,
                Status = t.Status,
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
