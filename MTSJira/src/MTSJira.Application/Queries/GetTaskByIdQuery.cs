using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;

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
            //var task = await _context.Tasks
            //    .Include(t => t.Author)
            //    .Include(t => t.Assignee)
            //    .Include(t => t.Subtasks)
            //    .Include(t => t.RelatedTasks)
            //    .ThenInclude(rt => rt.RelatedTask)
            //    .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            var task = await _taskRepository.GetTaskByIdAsync(request.Id);

            if (task == null)
                throw new Exception($"Task with ID {request.Id} not found");

            return new TaskDto
            {
                Id = task.Id,
                Assignee = task.Assignee,
                Author = task.Author,
                ParentTaskId = task.ParentTaskId,
                Priority = task.Priority,
                Status = task.Status,
                Title = task.Title,
            };
        }
    }
}
