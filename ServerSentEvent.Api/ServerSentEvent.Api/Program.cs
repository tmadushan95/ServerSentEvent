using ServerSentEvent.Api.Gen;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Server-Sent Events endpoint for Orders
// URL: /live-Order
// With Minimal APIs and built-in SSE support in .NET 10
app.Map("live-Order", (CancellationToken cancellationToken) =>
{
    static async IAsyncEnumerable<SseItem<Order>> GetOrder(
        [EnumeratorCancellation] CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(2000, ct);

            yield return new SseItem<Order>(OrderGen.CreateOrder(), "order")
            {
                ReconnectionInterval = TimeSpan.FromMinutes(1)
            };
        }
    }


    return TypedResults.ServerSentEvents(GetOrder(cancellationToken));
});

await app.RunAsync();
