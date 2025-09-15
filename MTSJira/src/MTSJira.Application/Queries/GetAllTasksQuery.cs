using MediatR;
using MTSJira.Application.InfrastructureContracts.Repositories;
using MTSJira.Application.Models.Task;
using System.Threading.Tasks;

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
            var tasks = await _taskRepository.GetAllTasksAsync();

            return tasks.Select(x => new TaskDto
            {
                Id = x.Id,
                Assignee = x.Assignee,
                Author = x.Author,
                ParentTaskId = x.ParentTaskId,
                Priority = x.Priority,
                Status = x.Status,
                Title = x.Title,
            });
        }
    }
}
