using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Repository.Contracts;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TaskTracker.DataAccess.Entities;

namespace TaskTracker_NSubstitute.Tests
{
    public class ProjectRepositoryNSTests
    {
        //private readonly IUnitOfWork unitOfWorkMock;
        private readonly IRepository<Project> _projectRepository;

        public ProjectRepositoryNSTests()
        {
            //unitOfWorkMock = Substitute.For<IUnitOfWork>();
            _projectRepository = Substitute.For<IRepository<Project>>();
        }

        
    }
}
