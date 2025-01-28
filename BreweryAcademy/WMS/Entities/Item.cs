using System.Text.Json.Serialization;

namespace WMS.Entities
{
    public class Item
    {
        [JsonIgnore] 
        public int InternalId { get; set; }
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
