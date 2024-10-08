using ContractChecker;
using ContractChecker.JsonConverters;
using TestWebApplication.Models;

namespace Microservices1.Models;

public record CityDTO : IContract
{
    public string Name { get; init; }

    [JsonOptional]
    public string Country { get; init; }

    public List<CompanyDTO> CompanyDTOs { get; init; }
    public CompanyDTO CompanyDTO { get; init; }
}