using System.Runtime.InteropServices.JavaScript;
using MovieApi.Models;
using Newtonsoft.Json;

namespace MovieApi.Clients;

public class OmdbService
{
    private readonly HttpClient _omdbApiClient;

    public OmdbService(HttpClient omdbApiClient)
    {
        _omdbApiClient = omdbApiClient;
    }

    public async Task<FilmResponse?> GetMovieByImdbIdAsync(string imdbId, CancellationToken ct = default)
    {
        var response =  await _omdbApiClient.GetAsync($"{_omdbApiClient.BaseAddress}i={imdbId}", ct);
        var jsonString = await response.Content.ReadAsStringAsync(ct);
        
        return JsonConvert.DeserializeObject<FilmResponse>(jsonString);
    }
}