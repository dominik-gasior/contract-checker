using System;
using System.Text.Json.Serialization;
using ContractChecker;

namespace Microservices2.Models;

public record CityDTO : IContract
{
    public string Name { get; init; }

    [JsonIgnore]
    public string Country { get; init; }

    public List<CompanyDTO> CompanyDTOs { get; init; }
    public CompanyDTO CompanyDTO { get; init; }
}
