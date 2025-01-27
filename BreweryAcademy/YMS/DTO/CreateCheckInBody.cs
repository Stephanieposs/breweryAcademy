namespace YMS.DTO
{
	public class CreateCheckInBody
	{
		public InvoiceDto Invoice { get; set; } = default!;
		public string DriverDocument { get; set; } = string.Empty;
		public string TruckPlate { get; set; } = string.Empty;
	}

	public class  InvoiceDto
	{
		public int InvoiceId { get; set; }
		public List<InvoiceItemDto> Items { get; set; } = new();
		public string InvoiceType { get; set; }
	}

	public class InvoiceItemDto
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
	}

	public class CreateCheckInResponse
	{
		public int Id { get; set; }
	}
}
