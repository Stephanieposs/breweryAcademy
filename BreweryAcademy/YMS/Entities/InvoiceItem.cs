﻿namespace YMS.Entities
{
	public class InvoiceItem
	{
        public int InvoiceItemId { get; set; }	
		public int InvoiceId { get; set; }
		public int ProductId { get; set; }
        public int Quantity { get; set; }
		
		public InvoiceItem() { }
    }
}
