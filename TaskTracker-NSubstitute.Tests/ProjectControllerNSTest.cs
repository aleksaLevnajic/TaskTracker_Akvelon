using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TaskTracker.API.Controllers;
using TaskTracker.API.DTO;
using TaskTracker.DataAccess.Entities;
using TaskTracker.DataAccess.Repository.Contracts;
using Task = TaskTracker.DataAccess.Entities.Task;

namespace TaskTracker_NSubstitute.Tests
{
    public class ProjectControllerNSTest
    {
        private readonly IUnitOfWork unitOfWorkMock;
        private readonly ProjectController projectController;

        public ProjectControllerNSTest()
        {
            unitOfWorkMock = Substitute.For<IUnitOfWork>();
            projectController = new ProjectController(unitOfWorkMock);
        }

        [Fact]
        public void GetAllProjects_ReturnsStatusCode200()
        {
            var result = projectController.GetAll() as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void GetSingleProjectById_ReturnsStatusCode200()
        {
            var project = new Project
            {
                Id = 15,
                Name = "Test",
                StartDate = new DateTime(2024, 12, 20),
                EndDate = new DateTime(2024, 12, 30),
                Status = ProjectStatus.NotStarted,
                Priority = 1//,
                //Tasks = new List<Task>()
            };
            //unitOfWorkMock.ProjectRepository.Get(project.Id).Returns(project);

            var result = projectController.GetById(project.Id) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void Post_CreateAProject_ShouldReturnStatusCode201()
        {
            CreateProjectDTO projectDtoMock = new CreateProjectDTO
            {
                Name = "Test",
                StartDate = new DateTime(2024, 12, 20),
                EndDate = new DateTime(2024, 12, 30),
                Priority = 1
            };
            var result = projectController.Post(projectDtoMock) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
        }
    }
}
