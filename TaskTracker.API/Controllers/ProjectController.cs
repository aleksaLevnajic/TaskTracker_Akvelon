using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using TaskTracker.API.DTO;
using TaskTracker.DataAccess.Entities;
using TaskTracker.DataAccess.Repository.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var projects = _unitOfWork.ProjectRepository.GetAll(x => x.Tasks);

            if(projects == null) 
                return NotFound();

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if(id <= 0)
                return BadRequest("Provided id has to be greater then 0.");

            var project = _unitOfWork.ProjectRepository.Get(id, x => x.Tasks);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateProjectDTO createProjectDto)
        {
            var errors = new List<string>();

            if(createProjectDto == null)
            {
                return BadRequest("You haven't sent data in a valid form.");
            }

            if(string.IsNullOrEmpty(createProjectDto.Name))
            {
                errors.Add("Name of a project is requierd.");
            }
            if(createProjectDto.StartDate < DateTime.UtcNow || createProjectDto.EndDate < DateTime.UtcNow)
            {
                errors.Add("You can't pick a date that has been passed.");
            }
            if(createProjectDto.StartDate > createProjectDto.EndDate)
            {
                errors.Add("Start date has to be before end date");
            }
            if(createProjectDto.Priority <= 0)
            {
                errors.Add("Priority must be greater than 0.");
            }

            if(errors.Any())
                return UnprocessableEntity(errors);

            try
            {
                var project = new Project
                {
                    Name = createProjectDto.Name,
                    StartDate = createProjectDto.StartDate,
                    EndDate = createProjectDto.EndDate,
                    Priority = createProjectDto.Priority,
                    Status = ProjectStatus.NotStarted
                };
                _unitOfWork.ProjectRepository.Add(project);
                _unitOfWork.Save();
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }
            

            return Created();

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProjectDTO project)
        {
            if (project == null || id <= 0)
                return NotFound();

            var projectToUpdate = _unitOfWork.ProjectRepository.Get(id);

            if (projectToUpdate != null)
            {
                try
                {
                    projectToUpdate.Name = project.Name;
                    projectToUpdate.StartDate = project.StartDate;
                    projectToUpdate.EndDate = project.EndDate;
                    projectToUpdate.Priority = project.Priority;
                    projectToUpdate.Status = project.Status;

                    _unitOfWork.ProjectRepository.Update(projectToUpdate);
                    _unitOfWork.Save();
                }
                catch(DbUpdateException ex)
                {
                    return StatusCode(500, new { error = "Internal server error.", ex.Message });
                }                
            }
            else
            {
                return UnprocessableEntity();
            }            

            return Ok(projectToUpdate);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Provided id has to be greater than 0.");

            var projectToDelete = _unitOfWork.ProjectRepository.Get(id);

            if (projectToDelete == null)
                return NotFound();

            try
            {
                _unitOfWork.ProjectRepository.Delete(projectToDelete);
                _unitOfWork.Save();
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }           

            return NoContent();
        }

        [HttpGet("tasks/{id}")]
        public IActionResult ViewAllTasks(int id)
        {
            if (id <= 0)
                return BadRequest("Provided id has to be greater than 0.");

            var tasksForProject = _unitOfWork.ProjectRepository.ViewAllTasks(id);

            if (tasksForProject == null)
                return UnprocessableEntity();

            List<ViewAllTasksDTO> tasksDtoList = new List<ViewAllTasksDTO>();
            try
            {
                foreach (var task in tasksForProject)
                {
                    tasksDtoList.Add(new ViewAllTasksDTO
                    {
                        Name = task.Name,
                        Description = task.Description,
                        Priority = task.Priority,
                        Status = task.Status
                    });
                }
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }            

            return Ok(tasksDtoList);
        }

        [HttpGet("filterDate/")]
        public IActionResult FilterByDate(DateTime? startDate, DateTime? endDate)
        {
            //If user doesn't pass in any value -> all projects will be returned
            //He can pass in either startDate or endDate or both and get projects with specific values returned

            if(startDate < DateTime.UtcNow || (endDate < DateTime.UtcNow && startDate < endDate))
                return BadRequest("Invalid dates provided.");

            IEnumerable<Project> projectsToReturn = new List<Project>();
            
            try
            {
                if (startDate == null && endDate == null)
                {
                    projectsToReturn = _unitOfWork.ProjectRepository.GetAll();
                }
                if (startDate != null && endDate == null)
                {
                    projectsToReturn = _unitOfWork.ProjectRepository.GetAllExp(x => x.StartDate == startDate);
                }
                else if (startDate == null && endDate != null)
                {
                    projectsToReturn = _unitOfWork.ProjectRepository.GetAllExp(x => x.EndDate == endDate);
                }
                else if (startDate != null && endDate != null)
                {
                    projectsToReturn = _unitOfWork.ProjectRepository.GetAllExp(x => x.StartDate == startDate && x.EndDate == endDate);
                }
            }
            catch(DbException ex)
            {
                return StatusCode(500, new { error = "Internal server error.", ex.Message });
            }            

            return Ok(projectsToReturn);
        }
    }
}
