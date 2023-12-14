using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DataAccess.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
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
