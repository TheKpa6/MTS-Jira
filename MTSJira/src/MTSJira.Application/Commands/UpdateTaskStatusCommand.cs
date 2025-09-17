using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;

namespace MTSJira.Application.Commands
{
    public class UpdateTaskStatusCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public UpdateTaskStatusRequest Request { get; set; } = null!;
    }

    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            await _taskRepository.UpdateTaskStatusAsync(request.Id, request.Request);

            return true;
        }
    }
}
