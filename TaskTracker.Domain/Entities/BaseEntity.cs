﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.DataAccess.Entities
{
    public class BaseEntity
    {
        //[JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
