﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.API.Controllers;
using TaskTracker.API.DTO;
using TaskTracker.DataAccess.Entities;
using TaskTracker.Tests.Mocks;

namespace TaskTracker.Tests
{
    public class ProjectControllerTests
    {
        [Fact]
        public void TestGetAllProject_ReturnProjectList()
        {
            var unitOfWorkMock = MockIUnitOfWork.GetMock();
            var projectController = new ProjectController(unitOfWorkMock.Object);

            var result = projectController.GetAll() as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        /*[Fact]
        public void TestGetProjectById_ReturnsSingleProject()
        {
            var unitOfWorkMock = MockIUnitOfWork.GetMock();
            var projectController = new ProjectController(unitOfWorkMock.Object);

            var result = projectController.GetById(111) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }*/

        [Fact]
        public void TestCreatingProject_AddToDb()
        {
            var unitOfWorkMock = MockIUnitOfWork.GetMock();
            var projectController = new ProjectController(unitOfWorkMock.Object);
            var project = new CreateProjectDTO
            {
                Name = "Test",
                StartDate = new DateTime(2023, 12, 12),
                EndDate = new DateTime(2023, 12, 20),
                Priority = 1
            };


            var result = projectController.Post(project) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.Created, result!.StatusCode);

        }
    }
}
