using Microsoft.AspNetCore.Mvc;

namespace ServerSentEvent.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        /// <summary>
        /// Streams periodic weather forecast updates to the client using Server-Sent Events (SSE).
        /// </summary>
        /// <remarks>The response is sent as a continuous stream with the 'text/event-stream' content
        /// type. The client receives updated weather forecast data every 5 seconds until the request is cancelled or
        /// the connection is closed. This endpoint is suitable for real-time or live-update scenarios where the client
        /// needs to receive ongoing data without polling.</remarks>
        /// <param name="cancellationToken">A token that can be used to request cancellation of the streaming operation. If cancellation is requested,
        /// the stream will end and the method will complete.</param>
        /// <returns>A task that represents the asynchronous operation of streaming weather forecast data to the client.</returns>
        [HttpGet("GetWeatherForecast")]
        public async Task Get(CancellationToken cancellationToken)
        {
            Response.Headers.Append("Content-Type", "text/event-stream");

            while (!cancellationToken.IsCancellationRequested)
            {
                var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray();

                // Convert to JSON
                var json = System.Text.Json.JsonSerializer.Serialize(forecast);

                // Send the event
                await Response.WriteAsync($"data: {json}\n\n", cancellationToken: cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);

                // Wait 5 seconds before sending the next update
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
