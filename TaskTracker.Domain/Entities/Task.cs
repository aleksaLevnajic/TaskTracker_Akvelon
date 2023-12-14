using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TaskTracker.DataAccess.Entities
{
    public class Task : BaseEntity
    {
        [Required(ErrorMessage = "Task Name is requird")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, 100, ErrorMessage = "Priority must be greater than 0.")]
        public int  Priority { get; set; }
        public TaskStatus Status { get; set; }
        [Required(ErrorMessage = "ProjectId is requird")]
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
