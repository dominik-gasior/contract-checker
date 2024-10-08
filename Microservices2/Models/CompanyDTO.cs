using ContractChecker;

namespace Microservices2.Models;

public class CompanyDTO : IContract
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public CompanyDTO Company { get; set; }

    // [JsonIgnore]
    // public string Country { get; init; }
}