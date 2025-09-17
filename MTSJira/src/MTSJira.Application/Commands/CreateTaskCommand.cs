using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;

namespace MTSJira.Application.Commands
{
    public class CreateTaskCommand : IRequest<int>
    {
        public CreateTaskRequest Request { get; set; }
        public string Author { get; set; }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var res = await _taskRepository.AddTaskAsync(request.Request);

            return res;
        }
    }
}
