using Newtonsoft.Json;

namespace TaskTracker_Cosmos.API.DataAccess.Entities
{
    public class BaseEntity
    {
        //[JsonProperty(PropertyName = "id")]
        public int Id {  get; set; }
    }
}
