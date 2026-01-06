using ServerSentEvent.Api.Gen;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS to allow requests from the React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactSSE", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001") // React URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Use CORS policy
app.UseCors("ReactSSE");

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
            try
            {
                await Task.Delay(2000, ct);
            }
            catch (TaskCanceledException)
            {
                yield break; 
            }

            yield return new SseItem<Order>(OrderGen.CreateOrder(), "order")
            {
                ReconnectionInterval = TimeSpan.FromMinutes(1)
            };
        }
    }


    return TypedResults.ServerSentEvents(GetOrder(cancellationToken));
});

await app.RunAsync();
