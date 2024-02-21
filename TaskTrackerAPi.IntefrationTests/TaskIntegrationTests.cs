using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Entities;
using Task = TaskTracker.DataAccess.Entities.Task;
using Newtonsoft.Json;

namespace TaskTrackerAPi.IntefrationTests
{
    public class TaskIntegrationTests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;
        private string _accessToken;
        private DateTime startDate = new DateTime(2024, 11, 11);
        private DateTime endDate = new DateTime(2024, 12, 12);
        //private string currentBearerToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiemlrYXppa2ljIiwiZXhwIjoxNzA4NTIyNzc0fQ.4eOjROr8uwr7tjl1jFiH55i72SF4IF9wa8FgWG42SPjVLtV1m-UHQOdDhdruhf4lLOIQFgOL8NBnidiDSOVrZw";


        public TaskIntegrationTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            //_client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentBearerToken}");

        }

        //copy GetAccessToken method from ProjectIntegrationTests when needed

        private void DataForTaskTesting()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<TaskTrackerDbContext>();
                dbContext.Database.EnsureDeleted();

                dbContext.Database.EnsureCreated();

                dbContext.Tasks.AddRange(new List<Task>
                {
                    new Task
                    {
                        Id = 98,
                        Name = "TestTask1",
                        Description = "Task description test 1",
                        Priority = 1,
                        Status = TaskTracker.DataAccess.Entities.TaskStatus.ToDo
                    },
                    new Task
                    {
                        Id = 99,
                        Name = "TestTask2",
                        Description = "Task description test 2",
                        Priority = 1,
                        Status = TaskTracker.DataAccess.Entities.TaskStatus.ToDo,
                        ProjectId = 999
                    }
                });
                dbContext.Projects.Add(new Project
                {
                    Id = 222,
                    Name = "Task project test",
                    StartDate = startDate,
                    EndDate = endDate,
                    Priority = 1,
                    Status = ProjectStatus.NotStarted
                });
                dbContext.SaveChanges();
            }
        }

        //INTEGRATION TESTS

        [Fact]
        public async System.Threading.Tasks.Task CheckGetAllEndpoint_Always_ReturnsStatusCodeOK()
        {
            var response = await _client.GetAsync("/api/Task");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_Always_AllTasks()
        {
            DataForTaskTesting();

            var response = await _client.GetAsync("api/Task");
            var result = await response.Content.ReadFromJsonAsync<List<Task>>();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Count.Should().Be(5);
            Assert.Collection(result! as IEnumerable<Task>,
                p =>
                {
                    Assert.Equal("Initial meeting", p.Name);
                    Assert.Equal("Present project.", p.Description);
                    Assert.Equal(1, p.Priority);
                    Assert.Equal(TaskTracker.DataAccess.Entities.TaskStatus.Done, p.Status);
                },
                p =>
                {
                    Assert.Equal("Develop App", p.Name);
                    Assert.Equal("Building software soultion for Task Tracker app.", p.Description);
                    Assert.Equal(2, p.Priority);
                    Assert.Equal(TaskTracker.DataAccess.Entities.TaskStatus.InProgress, p.Status);
                },
                p =>
                {
                    Assert.Equal("Final meeting", p.Name);
                    Assert.Equal("Final pre-production meeting.", p.Description);
                    Assert.Equal(3, p.Priority);
                    Assert.Equal(TaskTracker.DataAccess.Entities.TaskStatus.ToDo, p.Status);
                },
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

        [Fact]
        public async System.Threading.Tasks.Task CheckGetByIdEndpoint_Always_ReturnsStatusCodeOK()
        {
            DataForTaskTesting();
            int id = 99;

            var response = await _client.GetAsync($"/api/Task/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetById_Always_ReturnsTaskForProvidedId()
        {
            DataForTaskTesting();
            var id = 99;
            var taskSub = new Task
            {
                Id = 99,
                Name = "TestTask2",
                Description = "Task description test 2",
                Priority = 1,
                Status = TaskTracker.DataAccess.Entities.TaskStatus.ToDo,
                ProjectId = 999
            };

            var response = await _client.GetAsync($"/api/Task/{id}");
            var result = await response.Content.ReadFromJsonAsync<Task>();

            Assert.Equivalent(result, taskSub);
        }

        [Fact]
        public async System.Threading.Tasks.Task POSTEndpoint_ForGivenTask_Returns201Created()
        {
            DataForTaskTesting();
            var taskSub = new Task
            {
                //Id = 999,
                Name = "TestTask3",
                Description = "Task description test 3",
                Priority = 1,
                ProjectId = 222
            };

            var content = new StringContent(JsonConvert.SerializeObject(taskSub), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Task", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async System.Threading.Tasks.Task PUTEndpoint_ForGivenProject_Returns200OK()
        {
            DataForTaskTesting();
            var id = 99;
            var taskSub = new Task
            {
                Id = 99,
                Name = "TestTask2",
                Description = "Task description test 2",
                Priority = 1,
                Status = TaskTracker.DataAccess.Entities.TaskStatus.ToDo
            };

            var content = new StringContent(JsonConvert.SerializeObject(taskSub), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/Task/{id}", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async System.Threading.Tasks.Task DELETE_ForFivenTask_Retunrs204NoContent()
        {
            DataForTaskTesting();
            var id = 99;

            var response = await _client.DeleteAsync($"/api/Task/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }       
        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
