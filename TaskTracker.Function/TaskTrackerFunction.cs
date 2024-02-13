using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TaskTracker.Function
{
    public class TaskTrackerFunction
    {
        private readonly CosmosClient cosmosClient;
        private readonly IConfiguration configuration;

        public TaskTrackerFunction(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.cosmosClient = cosmosClient;
            this.configuration = configuration;
        }

        [FunctionName("TaskTrackerFunction")]
        //"0 0 */5 * * *" every 5h
        //"0 */5 * * * *" every 5 min
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {    
            string GetSecret(string secretName)
                => secretName + ": " + Environment.GetEnvironmentVariable(secretName, EnvironmentVariableTarget.Process);
            /*var accountEndpoint = configuration["AccountEndpoint"];
            var accountKey = configuration["AccountKey"];*/


            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("AccountEndpoint"), Environment.GetEnvironmentVariable("AccountKey"));
            //var cosmosClient = new CosmosClient(accountEndpoint, accountKey);
            var databaseName = "tasktracker-db";
            var containerName = "Projects";

            var cosmosContainer = cosmosClient.GetContainer(databaseName, containerName);

            var sqlConnectionString = Environment.GetEnvironmentVariable("SqlDatabaseConnectionString");
            //var sqlConnectionString = configuration["SqlDatabaseConnectionString"];
            //var sqlConnectionString = GetSecret("SqlDatabaseConnectionString");
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
                                Id = reader.GetInt32(reader.GetOrdinal("Id")).ToString(),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                Priority = reader.GetInt32(reader.GetOrdinal("Priority")),
                                Status = (ProjectStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                                Tasks = GetTasksFroProject(reader.GetInt32(reader.GetOrdinal("Id")), sqlConnection)
                            };

                            var jsonSettings = new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            };

                            var projectJson = JsonConvert.SerializeObject(project, jsonSettings);

                            await cosmosContainer.CreateItemAsync(projectJson);
                        }
                    }
                }
            }

            IEnumerable<object> GetTasksFroProject(int projectId, SqlConnection connection)
            {
                //var sqlConnection = new SqlConnection(sqlConnectionString);
                List<object> tasks = new List<object>();
                using (var sqlCommand2 = new SqlCommand($"SELECT * FROM Tasks WHERE ProjectId = {projectId}", connection))
                {
                    using (var reader = sqlCommand2.ExecuteReader())
                    {
                        while (reader.Read())
                        {                           

                            var task = new
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")).ToString(),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Priority = reader.GetInt32(reader.GetOrdinal("Priority")),
                                Status = (TaskStatus)reader.GetInt32(reader.GetOrdinal("Status")),
                                ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")), 
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
