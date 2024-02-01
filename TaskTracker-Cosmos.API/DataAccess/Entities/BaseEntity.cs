using Newtonsoft.Json;

namespace TaskTracker_Cosmos.API.DataAccess.Entities
{
    public class BaseEntity
    {
        //[JsonProperty("id")]
        public int Id {  get; set; }
    }
}
