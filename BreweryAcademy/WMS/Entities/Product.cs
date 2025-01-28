using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace WMS.Entities
{
    public class Product 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        [ValidateNever]
        [JsonIgnore]
        public ICollection<Stock> Stocks { get; set; }
    }
}
