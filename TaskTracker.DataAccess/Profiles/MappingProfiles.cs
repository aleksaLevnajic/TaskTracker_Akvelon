using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.API.DTO;
using TaskTracker.DataAccess.Entities;
using Task = TaskTracker.DataAccess.Entities.Task;


namespace TaskTracker.DataAccess.Profiles
{
    internal class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<AddTaskDTO, Task>().ReverseMap();
        }
    }
}
