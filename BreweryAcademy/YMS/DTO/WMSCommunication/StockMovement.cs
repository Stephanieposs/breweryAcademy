namespace YMS.DTO.WMSCommunication
{
	public class StockMovement
	{
		public int InvoiceId { get; set; }
		public int OperationType { get; set; }
		public List<Item> Products { get; set; }

		public StockMovement(int invoiceId, string operationType, List<Item> products)
		{
			InvoiceId = invoiceId;
			OperationType = operationType switch
			{
				"Load" => 1,
				"Unload" => 2,
				_ => 0
			};
			Products = products;
		}
	}
}
