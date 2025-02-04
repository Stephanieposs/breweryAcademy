using MassTransit;
using SAP4.Extensions;
using System.Text.Json;
using System.Text;
using MassTransit.Logging;
using SAP4.Entities;

namespace SAP4.Bus;

internal sealed class RecordRequestedEventConsumer : IConsumer<RecordRequestedEvent>
{
    private readonly ILogger<RecordRequestedEventConsumer> _logger;

    public RecordRequestedEventConsumer(ILogger<RecordRequestedEventConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<RecordRequestedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation("Processing Record: Id {Id}", message.Id);

        try
        {
            using var httpClient = new HttpClient();
            var notification = new { Status = message.Status.ToString() };
            var json = JsonSerializer.Serialize(notification);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"https://localhost:7071/api/Put/Invoice/{message.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"External API notified for InvoiceId: {message.Id}");
            }
            else
            {
                _logger.LogError($"Failed to notify external API for InvoiceId: {message.Id}. Status Code: {response.StatusCode}");
                // Handle error (retry, dead-letter queue, etc.)
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error notifying external API for InvoiceId: {message.Id}");
            // Handle error
        }
    }
}
