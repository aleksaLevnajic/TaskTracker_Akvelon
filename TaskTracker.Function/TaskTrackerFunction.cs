using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TaskTracker.Function
{
    public class TaskTrackerFunction
    {
        private readonly CosmosClient cosmosClient;

        public TaskTrackerFunction(CosmosClient cosmosClient)
        {
            this.cosmosClient = cosmosClient;
        }

        [FunctionName("TaskTrackerFunction")]
        //"0 0 */5 * * *" every 5h
        //"0 */5 * * * *" every 5 min
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var cosmosConnectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
            var cosmosClient = new CosmosClient(cosmosConnectionString);
            var databaseName = "tasktracker-db";
            var containerName = "Projects";

            var cosmosContainer = cosmosClient.GetContainer(databaseName, containerName);

            var sqlConnectionString = Environment.GetEnvironmentVariable("SqlDatabaseConnectionString");
            using (var sqlConnection = new SqlConnection(sqlConnectionString)) 
            { 
                await sqlConnection.OpenAsync();

                using(var sqlCommand = new SqlCommand("SELECT * FROM Projects", sqlConnection))
                {
                    using (var reader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var project = new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                Priority = reader.GetInt32(reader.GetOrdinal("Priority")),
                                Status = (ProjectStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                                Tasks = GetTasksFroProject(reader.GetInt32(reader.GetOrdinal("Id")), sqlConnection)
                            };
                            
                            var projectJson = JsonConvert.SerializeObject(project);

                            await cosmosContainer.CreateItemAsync(projectJson);
                        }
                    }
                }
            }

            IEnumerable<object> GetTasksFroProject(int projectId, SqlConnection connection)
            {
                //var sqlConnection = new SqlConnection(sqlConnectionString);
                List<object> tasks = new List<object>();
                using (var sqlCommand = new SqlCommand($"SELECT * FROM Tasks WHERE ProjectId = {projectId}", connection))
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {                           

                            var task = new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Priority = reader.GetInt32(reader.GetOrdinal("Priority")),
                                Status = (TaskStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                                ProjectId = reader.GetInt32(projectId)
                            };

                            tasks.Add(task);
                        }
                    }
                }

                return tasks;
            }


            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        
    }
    public enum ProjectStatus
    {
        NotStarted = 1,
        Active = 2,
        Completed = 3
    }
    public enum TaskStatus
    {
        ToDo = 1,
        InProgress = 2,
        Done = 3
    }

}
