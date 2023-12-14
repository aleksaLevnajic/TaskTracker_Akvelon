namespace TaskTracker.API.DTO
{
    public class CreateTaskDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int ProjectId { get; set; }
    }
}
