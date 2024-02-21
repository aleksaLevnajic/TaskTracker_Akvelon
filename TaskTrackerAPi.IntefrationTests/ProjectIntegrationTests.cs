

using Azure.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using NSubstitute;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TaskTracker.API;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Entities;
using TaskTrackerAPi.IntefrationTests.DbContext;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Task = TaskTracker.DataAccess.Entities.Task;

namespace TaskTrackerAPi.IntefrationTests
{
    public class ProjectIntegrationTests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private string _accessToken;
        private DateTime startDate = new DateTime(2024, 11, 11);
        private DateTime endDate = new DateTime(2024, 12, 12);
        private string currentBearerToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiemlrYXppa2ljIiwiZXhwIjoxNzA4NTM4NDI5fQ.lPeSQxN35i06XzptQd51OhisjJHupJcuoh4rF88z_NmRaorLp5hePfRx5f4kAD7YvsrKZHID-ls7go8I2eHQkA";

        private readonly MockTaskTrackerDbContext _mockDbContext;
        public ProjectIntegrationTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
            //_accessToken = GetAccessTokenAsync().Result;
            //_client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentBearerToken}");

            /*var options = new DbContextOptionsBuilder<TaskTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
             _mockDbContext = new MockTaskTrackerDbContext(options);*/

        }        

        private async Task<string> GetAccessTokenAsync()
        {
            var loginData = new { Username = "zikazikic", Password = "zika123" }; 
            var loginContent = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            var loginResponse = await _client.PostAsync("/api/Auth/login", loginContent);

            if (!loginResponse.IsSuccessStatusCode)
            {
                //throw new Exception($"Login failed with status code {loginResponse.StatusCode}");
            }

            var responseContent = await loginResponse.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<string>(responseContent);

            return responseData!; 
        }
        private void DataForProjectTesting()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<TaskTrackerDbContext>();
                dbContext.Database.EnsureDeleted();

                dbContext.Database.EnsureCreated();

                dbContext.Projects.AddRange(new List<Project>
                {
                    new Project { Id = 100, Name = "Test1", StartDate = startDate, EndDate = endDate, Priority = 1, Status = ProjectStatus.NotStarted},
                    new Project { Id = 101, Name = "Test2", StartDate = startDate, EndDate = endDate, Priority = 1, Status = ProjectStatus.NotStarted}
                });
                dbContext.SaveChanges();


            }
        }

        //INTEGRATION TESTS

        [Fact]
        public async System.Threading.Tasks.Task CheckGetAllEndpoint_Always_ReturnsStatusCodeOK()
        {
            var response = await _client.GetAsync("/api/Project");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_Always_AllProjects()
        {
            DataForProjectTesting();

            var response = await _client.GetAsync("api/Project");
            var result = await response.Content.ReadFromJsonAsync<List<Project>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Count.Should().Be(3);
            Assert.Collection(result! as IEnumerable<Project>,
                p =>
                {
                    Assert.Equal("Task Tracker App", p.Name);
                    //Assert.Equal(startDate.ToString(), p.StartDate.ToString());
                    //Assert.Equal(endDate.ToString(), p.EndDate.ToString());
                    Assert.Equal(1, p.Priority);
                    Assert.Equal(ProjectStatus.Active, p.Status);
                },
                p =>
                {
                    Assert.Equal("Test1", p.Name);
                    Assert.Equal(startDate.ToString(), p.StartDate.ToString());
                    Assert.Equal(endDate.ToString(), p.EndDate.ToString());
                    Assert.Equal(1, p.Priority);
                    Assert.Equal(ProjectStatus.NotStarted, p.Status);
                },
                p =>
                {
                    Assert.Equal("Test2", p.Name);
                    Assert.Equal(startDate.ToString(), p.StartDate.ToString());
                    Assert.Equal(endDate.ToString(), p.EndDate.ToString());
                    Assert.Equal(1, p.Priority);
                    Assert.Equal(ProjectStatus.NotStarted, p.Status);
                }
            );   
        }

        [Fact]
        public async System.Threading.Tasks.Task CheckGetByIdEndpoint_Always_ReturnsStatusCodeOK()
        {
            DataForProjectTesting();
            int id = 100;

            var response = await _client.GetAsync($"/api/Project/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetById_Always_ReturnsProjectForProvidedId()
        {
            DataForProjectTesting();
            var id = 100;
            //2024-01-16T15:56:11.5089075
            var projectSub = new Project
            {
                Id = 100,
                Name = "Test1",
                StartDate = startDate,
                EndDate = endDate,
                Priority = 1,
                Status = ProjectStatus.NotStarted
            };

            var response = await _client.GetAsync($"/api/Project/{id}");
            var result = await response.Content.ReadFromJsonAsync<Project>();

            //response.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.Equivalent(result, projectSub);
        }

        [Fact]
        public async System.Threading.Tasks.Task POSTEndpoint_ForGivenProject_Returns201Created()
        {
            var projectSub = new Project
            {
                //Id = 999,
                Name = "Test post",
                StartDate = startDate,
                EndDate = endDate,
                Priority = 1,
                Status = ProjectStatus.NotStarted
            };

            var content = new StringContent(JsonConvert.SerializeObject(projectSub), Encoding.UTF8, "application/json");            
            var response = await _client.PostAsync("/api/Project", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode); 
        }        

        [Fact]
        public async System.Threading.Tasks.Task PUTEndpoint_ForGivenProject_Returns200OK()
        {
            DataForProjectTesting();
            var projectSub = new Project
            {
                Id = 100,
                Name = "Test1",
                StartDate = startDate,
                EndDate = endDate,
                Priority = 1,
                Status = ProjectStatus.NotStarted
            };

            var content = new StringContent(JsonConvert.SerializeObject(projectSub), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Project/1", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async System.Threading.Tasks.Task DELETE_ForFivenProject_Retunrs204NoContent()
        {
            DataForProjectTesting();
            var id = 100;

            var response = await _client.DeleteAsync($"/api/Project/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTask_ForProvidedProjectId_ReturnsTasksWith200OK()
        {
            var id = 100;
            using (var scope = _factory.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<TaskTrackerDbContext>();

                dbContext.Database.EnsureCreated();

                var tasks = new List<Task>
                {
                    new Task
                    {
                        Id = 100,
                        Name = "TestTask1",
                        Description = "Task description test 1",
                        Priority = 1,
                        Status = TaskTracker.DataAccess.Entities.TaskStatus.ToDo
                    },
                    new Task
                    {
                        Id = 101,
                        Name = "TestTask2",
                        Description = "Task description test 2",
                        Priority = 1,
                        Status = TaskTracker.DataAccess.Entities.TaskStatus.ToDo
                    }
                };
                dbContext.Tasks.AddRange(tasks);

                dbContext.Projects.AddRange(new List<Project>
                {
                    new Project { Id = 100, Name = "Test1", StartDate = startDate, EndDate = endDate, Priority = 1, Status = ProjectStatus.NotStarted, Tasks = tasks}
                });
                dbContext.SaveChanges();
            }

            var response = await _client.GetAsync($"/api/Project/tasks/{id}");
            var result = await response.Content.ReadFromJsonAsync<List<Task>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Count.Should().Be(2);
            Assert.Collection(result! as IEnumerable<Task>,
                p =>
                {
                    Assert.Equal("TestTask1", p.Name);
                    Assert.Equal("Task description test 1", p.Description);
                    Assert.Equal(1, p.Priority);
                    Assert.Equal(TaskTracker.DataAccess.Entities.TaskStatus.ToDo, p.Status);
                },
                p =>
                {
                    Assert.Equal("TestTask2", p.Name);
                    Assert.Equal("Task description test 2", p.Description);
                    Assert.Equal(1, p.Priority);
                    Assert.Equal(TaskTracker.DataAccess.Entities.TaskStatus.ToDo, p.Status);
                }
            );
        }        
        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}