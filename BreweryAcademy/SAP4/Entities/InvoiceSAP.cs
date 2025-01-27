using System.ComponentModel.DataAnnotations;

namespace SAP4.Entities;

public class InvoiceSAP
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public InvoiceStatus Status { get; set; }

}
