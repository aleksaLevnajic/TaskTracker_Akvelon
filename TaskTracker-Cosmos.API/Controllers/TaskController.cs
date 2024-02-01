using Microsoft.AspNetCore.Mvc;
using TaskTracker_Cosmos.API.DataAccess.Repository.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskTracker_Cosmos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/<TaskController>
        [HttpGet]
        public IActionResult GetAll()
        {
            var tasks = _taskRepository.GetAll();
            if (tasks == null)
                return NotFound();

            return Ok(tasks);
        }

        // GET api/<TaskController>/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Provided id has to be greater than 0.");

            var task = _taskRepository.Get(id);
            if (task == null)
                return NotFound("There is no task with provided id.");

            return Ok(task);
        }

    }
}
