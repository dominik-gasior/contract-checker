namespace Microservices1.Clients;

internal class Microservices2Client
{
    private readonly HttpClient _httpClient;

    public Microservices2Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public HttpClient GetHttpClient() => _httpClient;
}
