using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IQuoteService, QuoteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var quotes = app.MapGroup("/api/quotes");

quotes.MapPost("/", (IQuoteService service, QuoteRequest req) => service.GetQuotes(req));

app.Run();

// DTOs
record QuoteRequest(string InsuranceType, int Age, int Coverage);
record QuoteResponse(string Provider, decimal Premium);

// Service
interface IQuoteService
{
    IEnumerable<QuoteResponse> GetQuotes(QuoteRequest request);
}

class QuoteService : IQuoteService
{
    private static readonly string[] Providers = ["AIG", "Star Health", "FutureGenerali"];

    public IEnumerable<QuoteResponse> GetQuotes(QuoteRequest request)
    {
        var rng = new Random();
        return Providers.Select(p => new QuoteResponse(p, rng.Next(100, 1000)));
    }
}
