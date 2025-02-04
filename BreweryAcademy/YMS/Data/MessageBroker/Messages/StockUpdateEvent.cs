using YMS.DTO.WMSCommunication;

namespace YMS.Data.MessageBroker.Messages
{
	public record StockUpdateEvent(int InvoiceId, int OperationType,List<Item> Products );
}
