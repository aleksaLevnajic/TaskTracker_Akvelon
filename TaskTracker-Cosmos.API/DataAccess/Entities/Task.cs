using System.ComponentModel.DataAnnotations;

namespace TaskTracker_Cosmos.API.DataAccess.Entities
{
    public class Task : BaseEntity
    {
        
        public string Name { get; set; }
        public string Description { get; set; }

        public int Priority { get; set; }
        public TaskStatus Status { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }

    public enum TaskStatus
    {
        ToDo = 1,
        InProgress = 2,
        Done = 3
    }
}
