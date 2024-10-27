namespace Microservices1.Models;

public record CityDTO
{
    public string Name { get; init; }
    public string Country { get; init; }

    public List<CompanyDTO> CompanyDTOs { get; init; }
    public CompanyDTO CompanyDTO { get; init; }
}