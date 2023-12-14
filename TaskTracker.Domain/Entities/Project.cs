using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DataAccess.Entities
{
    public class Project : BaseEntity
    {
        [Required(ErrorMessage = "Project Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Start date is requierd.")]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Range(1, 100, ErrorMessage = "Priority must be greater than 0.")]
        public int Priority { get; set; }
        public ProjectStatus Status { get; set; }

        public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    }

    public enum ProjectStatus
    {
        NotStarted = 1,
        Active = 2,
        Completed = 3
    }
}
