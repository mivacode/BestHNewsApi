using BestHNewsApi.Clients;
using BestHNewsApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<HackerNewsApiClientOptions>(
    builder.Configuration.GetSection(nameof(HackerNewsApiClientOptions)));

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
