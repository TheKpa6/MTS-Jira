using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;

namespace MTSJira.Application.Commands
{
    public class UpdateTaskCommand : IRequest<TaskDto>
    {
        public int Id { get; set; }
        public UpdateTaskRequest Request { get; set; } = null!;
    }

    public class UpdateTaskWithRelationsCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskWithRelationsCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var taskData = await _taskRepository.Update(request.Id, request.Request);

            return new TaskDto
            {
                Id = taskData.Id,
                Title = taskData.Title,
                ParentTaskId = taskData.ParentTaskId,
                Assignee = taskData.Assignee,
                Author = taskData.Author,
                Priority = taskData.Priority,
                Status = taskData.Status,
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
