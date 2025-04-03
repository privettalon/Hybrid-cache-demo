using Microsoft.Extensions.Caching.Hybrid;
using MovieApi.Clients;
using MovieApi.Infrastructure;
using MovieApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<OmdbApiSettings>(builder.Configuration.GetSection("OmdbApi"));
builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection("Redis"));

builder.Services.AddHttpClient<OmdbService>(client =>
{
    var settings = builder.Configuration.GetSection("OmdbApi").Get<OmdbApiSettings>();
    client.BaseAddress = new Uri(settings?.BaseUrl ?? throw new InvalidOperationException("BaseUrl not set"));
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    var settings = builder.Configuration.GetSection("Redis").Get<RedisConfig>();
    options.Configuration = new string(settings?.Configuration
                                       ?? throw new InvalidOperationException("Redis Configuration not set"));
    options.InstanceName = new string( settings?.InstanceName
                                       ?? "RedisTestInstance");
});

builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        LocalCacheExpiration = TimeSpan.FromMinutes(1),
        Expiration = TimeSpan.FromMinutes(5),
    };
});


var app = builder.Build();
app.UseHttpsRedirection();

app.MapGet("/movies/{imdb}", async (
        string imdb,
        OmdbService client,
        HybridCache hybridCache,
        CancellationToken cancellationToken) => 
{
    var movie = await hybridCache.GetOrCreateAsync($"movies:{imdb}", async ct =>
    {
        var movie = await client.GetMovieByImdbIdAsync(imdb, ct);
        return movie;
    },
    tags: ["movies"],
    cancellationToken: cancellationToken);

    return movie is not null
        ? Results.Ok(movie)
        : Results.NotFound();
})
.WithName("GetMovies")
.WithTags("Movies")
.WithOpenApi();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies API"); });
}

app.Run();