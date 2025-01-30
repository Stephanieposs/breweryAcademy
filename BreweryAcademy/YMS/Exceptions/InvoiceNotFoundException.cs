namespace YMS.Exceptions
{
	public class InvoiceNotFoundException:NotFoundException
	{
		public InvoiceNotFoundException(int id):base("Invoice", id) { 
		}
	}
}
