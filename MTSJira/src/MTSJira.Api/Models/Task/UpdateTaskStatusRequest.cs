using MTSJira.Api.Models.Task.Enums;

namespace MTSJira.Api.Models.Task
{
    public class UpdateTaskStatusRequest
    {
        public Enums.TaskStatus TaskStatus { get; set; }
    }
}
