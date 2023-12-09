using BestHNewsApi.Clients;
using BestHNewsApi.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<HackerNewsApiClientOptions>(
    builder.Configuration.GetSection(nameof(HackerNewsApiClientOptions)));

builder.Services.AddHttpClient("HackerNewsAPI", client =>
{
    var apiOptions = builder.Configuration.GetSection(nameof(HackerNewsApiClientOptions)).Get<HackerNewsApiClientOptions>();
    client.BaseAddress = new Uri(uriString: apiOptions.BaseUrl);
})
.AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
{
    TimeSpan.FromSeconds(1),
    TimeSpan.FromSeconds(5),
    TimeSpan.FromSeconds(10)
}))
.AddPolicyHandler(builder =>
{
    //allow max 25 concurrent requests to HackerNewsAPI via this named HttpClient
    return Policy.BulkheadAsync(25, int.MaxValue).AsAsyncPolicy<HttpResponseMessage>();
});


builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IHackerNewsApiClient, HackerNewsApiClient>();
builder.Services.AddSingleton<IHackerNewsApiClientCached, HackerNewsApiClientCached>();
builder.Services.AddScoped<IBestNewsService, BestNewsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
