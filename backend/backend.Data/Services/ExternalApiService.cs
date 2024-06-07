namespace backend.Data.Services;

public interface IExternalApiService
{
    Task<string> GetExternalDataAsync();
}

public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;

    public ExternalApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetExternalDataAsync()
    {
        var response = await _httpClient.GetAsync("https://api.example.com/data");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
