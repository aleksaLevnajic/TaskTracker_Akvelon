using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Entities;
using TaskTracker.DataAccess.Repository.Contracts;
using Task = TaskTracker.DataAccess.Entities.Task;

namespace TaskTracker.Tests.Mocks
{
    internal class MockIProjectRepository
    {
        public static Mock<IProjectRepository> GetMock()
        {
            var mock = new Mock<IProjectRepository>();

            //var project1 = new Project();
            var projects = new List<Project>()
            {
                new Project
                {
                    Id = 111,
                    Name = "Project Unit Test",
                    StartDate = DateTime.Now.AddYears(-20),
                    EndDate = DateTime.Now.AddDays(-19),
                    Priority = 1,
                    Status = ProjectStatus.NotStarted,
                    Tasks = new List<Task>()
                    {
                        new Task
                        {
                            Id = 111,
                            Name = "Task for Project Unit Test",
                            Description = "Unit testing",
                            Priority = 1,
                            Status = DataAccess.Entities.TaskStatus.ToDo,
                            ProjectId = 111//,
                            //Project = project1
                        }
                    }
                }
            };

            /*mock.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<Expression<Func<Project, object>>>())).Returns((int id) 
                => projects.FirstOrDefault(p => p.Id == id));*/
            mock.Setup(m => m.GetAll()).Returns(() => projects);
            mock.Setup(m => m.Add(It.IsAny<Project>())).Callback(() 
                => { return; });
            mock.Setup(m => m.Update(It.IsAny<Project>())).Callback(()
                => { return; });
            mock.Setup(m => m.Delete(It.IsAny<Project>())).Callback(()
                => { return; });
            mock.Setup(m => m.ViewAllTasks(It.IsAny<int>())).Returns((int id)
                => projects.Where(p => p.Id == id).SelectMany(t => t.Tasks));

            return mock;
        }
    }
}
