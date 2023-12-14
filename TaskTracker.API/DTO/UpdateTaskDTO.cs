﻿using TaskTracker.DataAccess.Entities;
using TaskStatus = TaskTracker.DataAccess.Entities.TaskStatus;

namespace TaskTracker.API.DTO
{
    public class UpdateTaskDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public TaskStatus Status { get; set; }
        public int ProjectId { get; set; }
    }
}
