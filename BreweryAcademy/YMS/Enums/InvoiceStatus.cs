using System.ComponentModel;

namespace YMS.Enums
{
	public enum InvoiceStatus
	{
		[Description("Unknown")]
		Unknown = 0,
		[Description("Active")]
		Active,
		[Description("Inactive")]
		Inactive
	}
	
}
