
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json.Serialization;
using TaskTracker.API.Swagger;
using TaskTracker.DataAccess.Data;
using TaskTracker.DataAccess.Repository.Contracts;
using TaskTracker.DataAccess.Repository.Repositories;
using TaskTracker.DataAccess.Profiles;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;

namespace TaskTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    //ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Token").Value!))
                };
            });

            /*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));*/


            /*var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Production.json", false).Build();
            builder.Services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                    .AddAzureAD(options => config.Bind("AzureAd", options));*/

            /*var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Production.json", false).Build();

            builder.Services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(options => config.Bind("AzureAd", options));*/
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("https://tasktrackerapi-live-project.azurewebsites.net/swagger/index.html") 
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });



            builder.Services.AddAuthorization();

            builder.Services.AddControllers();

            static void KeyVaultConfiguration(WebApplicationBuilder builder)
            {
                var keyVaultUrl = builder.Configuration["KeyVaultUrl"]!;
                var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
                builder.Services.AddSingleton(secretClient);

                string secretConnString = builder.Configuration["SecretName"]!;
                KeyVaultSecret secret = secretClient.GetSecret(secretConnString);
                builder.Configuration[secretConnString] = secret.Value;
                //add foreach so you can add jwt token
            }
            //KeyVaultConfiguration(builder);

            /*builder.Services.AddDbContext<TaskTrackerDbContext>(options =>
                options.UseSqlServer(builder.Configuration[builder.Configuration["SecretName"]!]!));*/

            builder.Services.AddDbContext<TaskTrackerDbContext>(options =>
                options.UseSqlServer(
                builder.Configuration.GetConnectionString("SqlServerCS")));            

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            builder.Services.AddControllers().AddJsonOptions(opt =>
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
                    securityScheme: new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "Enter Bearer Token(Example: Bearer 12345asdfg)",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }

                        }, new string[]{}
                    }
                });
            });

            //builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment()) //added ! to test for production stage
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}
            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        
    }
}
