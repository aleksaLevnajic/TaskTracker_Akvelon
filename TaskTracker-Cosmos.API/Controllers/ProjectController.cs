using Microsoft.AspNetCore.Mvc;
using TaskTracker_Cosmos.API.DataAccess.Repository.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskTracker_Cosmos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        public ProjectController(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var projects = _projectRepository.GetAll();

            foreach(var project in projects)
            {
                var tasks = _taskRepository.GetAllExp(x => x.ProjectId == project.Id).ToList();
                project.Tasks = tasks;
            }

            if (projects == null)
                return NotFound();

            return Ok(projects);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Provided id has to be greater then 0.");

            var project = _projectRepository.Get(id);
            var tasks = _taskRepository.GetAllExp(x => x.ProjectId == id).ToList();
            if(tasks.Count != 0)
                project.Tasks = tasks;

            if (project == null)
                return NotFound();

            return Ok(project);
        }
        
    }
}
