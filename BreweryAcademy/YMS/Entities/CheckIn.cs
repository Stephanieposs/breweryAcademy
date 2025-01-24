namespace YMS.Entities
{
	public class CheckIn
	{
		public int Id { get; set; }
		public int InvoiceReferenced { get; set; }
		public string DriverDocument { get; set; } = string.Empty;
		public string TruckPlate = string.Empty;
		public virtual Invoice Invoice { get; set; } = default!;

		public CheckIn()
		{


		}
	}
}
