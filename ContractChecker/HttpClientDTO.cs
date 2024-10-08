namespace ContractChecker;

public record HttpClientDTO
{
    public HttpClient HttpClient { get; init; }
    public string Endpoint { get; init; }
}
