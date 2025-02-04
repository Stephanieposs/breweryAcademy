using MassTransit;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.Json;
using System.Text;
using WMS.Entities;
using WMS.Extensions;

namespace WMS.Bus
{
    public sealed class InvoiceRequestedEventConsumer : IConsumer<InvoiceRequestedEvent>
    {
        private readonly ILogger<InvoiceRequestedEventConsumer> _logger;

        public InvoiceRequestedEventConsumer(ILogger<InvoiceRequestedEventConsumer> logger)
        {
            _logger = logger;
        }

        async Task IConsumer<InvoiceRequestedEvent>.Consume(ConsumeContext<InvoiceRequestedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation("Processing Invoice: Id {Id}", message.id);

            try
            {
                using var httpClient = new HttpClient();

                var notification = new { InvoiceId = message.id};  //mais itens depois
                var json = JsonSerializer.Serialize(notification);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.GetAsync($"https://localhost:7046/api/Sap/wms/{message.id}");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"SAP notified for InvoiceId: {message.id}");
                }
                else
                {
                    _logger.LogError($"Failed to notify SAP for InvoiceId: {message.id}. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error notifying SAP for InvoiceId: {message.id}");
            }
        }
    }
}
