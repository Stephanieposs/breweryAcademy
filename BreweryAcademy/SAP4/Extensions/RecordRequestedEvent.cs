using SAP4.Entities;

namespace SAP4.Extensions;

internal sealed record RecordRequestedEvent(int Id, InvoiceStatus Status);
