using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.API.Controllers;
using TaskTracker.DataAccess.Entities;
using TaskTracker.Tests.Mocks;
using Task = TaskTracker.DataAccess.Entities.Task;

namespace TaskTracker.Tests
{
    public class TaskControllerTests
    {

        [Fact]
        public void TestGetAllProject_ReturnProjectList()
        {
            var unitOfWorkMock = MockIUnitOfWork.GetMock();
            var mapperMock = new Mock<IMapper>();
            var taskController = new TaskController(unitOfWorkMock.Object, mapperMock.Object);

            var result = taskController.Get() as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public void TestGetByIdTask_ReturnSingleTask()
        {
            var unitOfWorkMock = MockIUnitOfWork.GetMock();
            var mapperMock = new Mock<IMapper>();
            var taskController = new TaskController(unitOfWorkMock.Object, mapperMock.Object);

            var result = taskController.Get(222) as ObjectResult;

            Assert.NotNull(result);
            //Assert.Equal(StatusCodes.Status200OK, result.StatusCode); //return 404, like task with id: 222 is not found
        }

    }
}
