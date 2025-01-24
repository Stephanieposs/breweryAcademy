using System.ComponentModel;

namespace YMS.Enums
{
	public enum InvoiceType
	{
		[Description("None")]
		None = 0,
		[Description("Entry")]
		Entry,
		[Description("Exist")]
		Exist
	}
}
