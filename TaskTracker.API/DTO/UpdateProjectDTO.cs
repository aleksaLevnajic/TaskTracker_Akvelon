using TaskTracker.DataAccess.Entities;

namespace TaskTracker.API.DTO
{
    public class UpdateProjectDTO
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Priority { get; set; }
        public ProjectStatus Status { get; set; }
    }
}
