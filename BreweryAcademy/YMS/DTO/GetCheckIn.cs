namespace YMS.DTO
{
	public class GetCheckIn
	{
		public int Id { get; set; }
		public InvoiceDto Invoice { get; set; } = default!;
		public string DriverDocument { get; set; } = string.Empty;
		public string TruckPlate { get; set; } = string.Empty;
	}
	public class GetAllCheckInsResponse
	{
		public List<GetCheckIn> Items { get; set; } = new();
	}
}
