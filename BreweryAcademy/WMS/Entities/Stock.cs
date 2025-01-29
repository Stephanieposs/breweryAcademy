using WMS.Enums;
namespace WMS.Entities
{
    public class Stock
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public OperationType OperationType { get; set; }
        public List<Item> Products { get; set; } = new List<Item>();
    }
}