using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTracker.API.DTO;
using TaskTracker.DataAccess.Entities;
using TaskTracker.DataAccess.Repository.Contracts;
using Task = TaskTracker.DataAccess.Entities.Task;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskTracker.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var tasks = _unitOfWork.TaskRepository.GetAll();
            if (tasks == null)
                return NotFound();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if(id <= 0)
                return BadRequest("Provided id has to be greater than 0.");

            var task = _unitOfWork.TaskRepository.Get(id);
            if (task == null)
                return NotFound("There is no task with provided id.");

            return Ok(task);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateTaskDTO createTaskDto)
        {
            var errors = new List<string>();

            if (createTaskDto == null)
                return Conflict();

            if(string.IsNullOrEmpty(createTaskDto.Name))
            {
                errors.Add("Task name can't be empty.");
            }
            if (string.IsNullOrEmpty(createTaskDto.Description))
            {
                errors.Add("Task description can't be empty.");
            }
            if (createTaskDto.Priority <= 0)
            {
                errors.Add("Task priority must be greater than 0.");
            }
            if(createTaskDto.ProjectId <= 0)
            {
                errors.Add("Project id must be greater than 0.");
            }

            var exsistingProjectId = _unitOfWork.ProjectRepository.GetAll().Any(x => x.Id == createTaskDto.ProjectId);
            if(!exsistingProjectId)
            {
                errors.Add("There are no projects with that id.");
            }
            if(errors.Any())
                return UnprocessableEntity(errors);

            try
            {
                var task = new Task
                {
                    Name = createTaskDto.Name,
                    Description = createTaskDto.Description,
                    Priority = createTaskDto.Priority,
                    Status = DataAccess.Entities.TaskStatus.ToDo,
                    ProjectId = createTaskDto.ProjectId,
                    Project = _unitOfWork.ProjectRepository.GetExp(x => x.Id == createTaskDto.ProjectId)
                };

                _unitOfWork.TaskRepository.Add(task);
                _unitOfWork.Save();
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }            

            return Created();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateTaskDTO updateTaskDto)
        {
            if (updateTaskDto == null || id <= 0)
                return NotFound();

            var taskToUpdate = _unitOfWork.TaskRepository.Get(id);

            try
            {
                if (taskToUpdate != null)
                {
                    taskToUpdate.Name = updateTaskDto.Name;
                    taskToUpdate.Description = updateTaskDto.Description;
                    taskToUpdate.Priority = updateTaskDto.Priority;
                    taskToUpdate.Status = updateTaskDto.Status;
                    taskToUpdate.ProjectId = updateTaskDto.ProjectId;

                    _unitOfWork.TaskRepository.Update(taskToUpdate);
                    _unitOfWork.Save();
                }
                else
                {
                    return NotFound();
                }
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }            

            return Ok(taskToUpdate);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Provided id has to be greater than 0.");

            var taskToDelete = _unitOfWork.TaskRepository.Get(id);
            if (taskToDelete == null)
                return NotFound();

            try
            {
                _unitOfWork.TaskRepository.Delete(taskToDelete);
                _unitOfWork.Save();
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }            

            return NoContent();
        }

        [HttpPost("addTask/")]
        public IActionResult AddTaskToProject(int projectId, AddTaskDTO taskToAdd)
        {
            if (projectId <= 0)
                return BadRequest("Project id has to be greater than zero.");
            if (_unitOfWork.ProjectRepository.Get(projectId) == null)
                return BadRequest("Project with that id doesen't exit.");
            if (taskToAdd == null)
                return BadRequest("You need to add task.");

            try
            {
                //Task task = _mapper.Map<Task>(taskToAdd);
                Task task = new Task
                {
                    Name = taskToAdd.Name,
                    Description = taskToAdd.Description,
                    Priority = taskToAdd.Priority,
                    Status = taskToAdd.Status,
                    ProjectId = projectId,
                    Project = _unitOfWork.ProjectRepository.GetExp(x => x.Id == projectId)
                };

                _unitOfWork.TaskRepository.AddTask(projectId, task);
                _unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }            

            return Created();
        }

        [HttpDelete("removeTask/")]
        public IActionResult RemoveTaskFromProject(int projectId, int taskId)
        {
            if (projectId <= 0)
                return BadRequest("Project id has to be greater than zero.");
            if (_unitOfWork.ProjectRepository.Get(projectId) == null)
                return BadRequest("Project with that id doesen't exit.");
            if (taskId <= 0)
                return BadRequest("Task id has to be grather than zero.");
            if (_unitOfWork.TaskRepository.Get(taskId) == null)
                return BadRequest("Task with that id doesen't exit.");

            try
            {
                _unitOfWork.TaskRepository.RemoveTask(projectId, taskId);
                _unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }

            return NoContent();
        }

        /*[HttpDelete("removeTask/")]
        public IActionResult RemoveTaskFromProject(int projectId, Task taskToRemove)
        {
            if (projectId <= 0)
                return BadRequest("Project id has to be grather than zero.");
            if (_unitOfWork.ProjectRepository.Get(projectId) == null)
                return BadRequest("Project with that id doesen't exit.");
            if (taskToRemove == null)
                return BadRequest("You need to pass thourgh a task.");

            try
            {
                _unitOfWork.TaskRepository.RemoveTask(projectId, taskToRemove);
                _unitOfWork.Save();
            }
            catch(InvalidOperationException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }

            return NoContent();
        }*/
    }
}
