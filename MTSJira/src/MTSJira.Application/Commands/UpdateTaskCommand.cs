using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;

namespace MTSJira.Application.Commands
{
    public class UpdateTaskCommand : IRequest<int>
    {
        public UpdateTaskRequest Request { get; set; }
        public int Id { get; set; }
    }

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, int>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<int> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var taskData = new Domain.Entities.TaskData
            {
                Id = request.Id,
                Assignee = request.Request.Assignee,
                Author = request.Request.Author,
                ParentTaskId = request.Request.ParentTaskId,
                Priority = request.Request.Priority,
                Status = request.Request.Status,
                Title = request.Request.Title,
            };

            await _taskRepository.UpdateTaskAsync(taskData);

            return taskData.Id;
        }
    }
}
