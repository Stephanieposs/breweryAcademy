using System.ComponentModel;

namespace YMS.Enums
{
	public enum InvoiceType
	{
		[Description("None")]
		None = 0,
		[Description("Load")]
		Load=1,
		[Description("Unload")]
		Unload=2
	}
}
