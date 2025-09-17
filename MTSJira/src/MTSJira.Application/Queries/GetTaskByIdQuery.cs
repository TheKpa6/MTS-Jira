using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;
using MTSJira.Application.Models.Task.Enums;
using MTSJira.Domain.Common.Enums;
using MTSJira.Domain.Exceptions;

namespace MTSJira.Application.Queries
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public int Id { get; set; }
    }

    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var taskData = await _taskRepository.GetTaskByIdAsync(request.Id);

            if (taskData == null)
                throw new JiraApplicationException($"Task with id {request.Id} not found", CommonErrorCode.ObjectNotFound);

            return new TaskDto
            {
                Id = taskData.Id,
                Title = taskData.Title,
                ParentTaskId = taskData.ParentTaskId,
                Assignee = taskData.Assignee,
                Author = taskData.Author,
                Priority = Enum.Parse<TaskPriority>(taskData.Priority.ToString()),
                Status = Enum.Parse<Models.Task.Enums.TaskStatus>(taskData.Status.ToString()),
                SubtasksIds = taskData.Subtasks.Select(t => taskData.Id).ToList(),
                RelatedTasksIds = taskData.RelatedTasks.Select(t => new TaskRelationshipDto
                {
                    RelatedTaskId = t.RelatedTaskId,
                    SourceTaskId = t.SourceTaskId,
                }).ToList(),
                RelatedToTasksIds = taskData.RelatedToTasks.Select(t => new TaskRelationshipDto
                {
                    RelatedTaskId = t.RelatedTaskId,
                    SourceTaskId = t.SourceTaskId,
                }).ToList(),
            };
        }
    }
}
