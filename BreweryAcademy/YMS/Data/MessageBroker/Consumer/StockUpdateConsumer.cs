using YMS.Data.MessageBroker.Messages;

namespace YMS.Data.MessageBroker.Consumer
{
	public class StockUpdateConsumer : IConsumer<StockUpdateEvent>
	{
		public async Task Consume(ConsumeContext<StockUpdateEvent> context)
		{
			var stock = context.Message;
			Console.WriteLine($"Consuming stock update for invoice {stock.InvoiceId} ");
			await Task.Delay(1000);

			Console.WriteLine($"Finished consumption for invoice {stock.InvoiceId}" );
			foreach(var item in stock.Products)
			{
				Console.WriteLine(item.Id);
			}
		
		}
	}
}
