namespace ContractChecker;

public record ContractCheckerConfiguration
{
    public required string ServiceName { get; init; }
    public required Type HttpClient { get; init; }
    public required string Endpoint { get; init; }
    public required IEnumerable<ContractDTO> ContractDTOs { get; init; }
}

public record ContractDTO(Type SourceName, string DestinationName);