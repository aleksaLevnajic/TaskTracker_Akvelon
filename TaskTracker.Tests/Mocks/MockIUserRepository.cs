using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Entities;
using TaskTracker.DataAccess.Repository.Contracts;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Tests.Mocks
{
    internal class MockIUserRepository
    {
        public static Mock<IUserRepository> GetMock()
        {
            var mock = new Mock<IUserRepository>();

            var users = new List<User>()
            {
                new User
                {
                    Id = 333,
                    FirstName = "Unit",
                    LastName = "Test",
                    Username = "UnitTest",
                    PasswordHash = "aaaaaa"
                }
            };

            /*mock.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<Expression<Func<User, object>>>())).Returns((int id) 
                =>  users.FirstOrDefault(u => u.Id == id));*/
            mock.Setup(m => m.GetAll()).Returns(() => users);
            mock.Setup(m => m.Add(It.IsAny<User>())).Callback(() 
                => { return; });
            mock.Setup(m => m.Update(It.IsAny<User>())).Callback(()
                => { return; });
            mock.Setup(m => m.Delete(It.IsAny<User>())).Callback(()
                => { return; });


            return mock;
        }
    }
}
