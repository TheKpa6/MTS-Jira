using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Domain.Common.Enums;
using MTSJira.Domain.Exceptions;

namespace MTSJira.Application.Commands
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var taskData = await _taskRepository.GetTaskByIdAsync(request.Id);

            if (taskData == null)
                throw new JiraApplicationException($"Task with id {request.Id} not found", CommonErrorCode.ObjectNotFound);

            await _taskRepository.DeleteTaskAsync(taskData);

            return true;
        }
    }
}
