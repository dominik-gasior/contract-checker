using ContractChecker;
using ContractChecker.JsonConverters;

namespace TestWebApplication.Models;

public record CompanyDTO : IContract
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public CompanyDTO Company { get; set; }

    [JsonOptional]
    public string Country { get; init; }
}