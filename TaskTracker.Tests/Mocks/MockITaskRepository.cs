using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Repository.Contracts;
using TaskTracker.DataAccess.Entities;
using Task = TaskTracker.DataAccess.Entities.Task;
using System.Linq.Expressions;

namespace TaskTracker.Tests.Mocks
{
    internal class MockITaskRepository
    {
        public static Mock<ITaskRepository> GetMock()
        {
            var mock = new Mock<ITaskRepository>();

            var tasks = new List<Task>()
            {
                new Task()
                {
                    Id = 222,
                    Name = "Task Unit Test",
                    Description = "Unit testing",
                    Priority = 1,
                    Status = DataAccess.Entities.TaskStatus.ToDo,
                    ProjectId = 111
                }
            };

            /*mock.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<Expression<Func<Task, object>>>())).Returns((int id) 
                => tasks.FirstOrDefault(t => t.Id == id));*/
            mock.Setup(m => m.GetAll()).Returns(tasks); //
            mock.Setup(m => m.Add(It.IsAny<Task>())).Callback(()
                => { return; });
            mock.Setup(m => m.Update(It.IsAny<Task>())).Callback(()
                => { return; });
            mock.Setup(m => m.Delete(It.IsAny<Task>())).Callback(()
                => { return; });

            mock.Setup(m => m.AddTask(It.IsAny<int>(), It.IsAny<Task>())).Callback(() 
                => { return; });
            mock.Setup(m => m.RemoveTask(It.IsAny<int>(), It.IsAny<int>())).Callback(()
                => { return; });


            return mock;
        }
    }
}
