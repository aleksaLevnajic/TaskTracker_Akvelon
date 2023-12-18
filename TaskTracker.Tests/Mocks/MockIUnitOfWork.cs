using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Repository.Contracts;

namespace TaskTracker.Tests.Mocks
{
    internal class MockIUnitOfWork
    {
        public static Mock<IUnitOfWork> GetMock()
        {
            var mock = new Mock<IUnitOfWork>();
            var projectRepoMock = MockIProjectRepository.GetMock();
            var taskRepoMock = MockITaskRepository.GetMock();
            var userRepoMock = MockIUserRepository.GetMock();

            mock.Setup(m => m.ProjectRepository).Returns(() => projectRepoMock.Object);
            mock.Setup(m => m.TaskRepository).Returns(() => taskRepoMock.Object);
            mock.Setup(m => m.UserRepository).Returns(() => userRepoMock.Object);
            mock.Setup(m => m.Save()).Callback(() => { return; });

            return mock;
        }
    }
}
