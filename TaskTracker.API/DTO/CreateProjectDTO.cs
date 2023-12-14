namespace TaskTracker.API.DTO
{
    public class CreateProjectDTO
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
    }
}
