using System;
using ContractChecker;
using Microservices1.Clients;

namespace Microservices1;

internal class CustomContractCheckerHealthCheck : ContractCheckerHealthCheck
{
    private readonly Microservices2Client microservices2Client;

    public CustomContractCheckerHealthCheck(Microservices2Client microservices2Client)
    {
        this.microservices2Client = microservices2Client;
    }

    protected override Dictionary<string, HttpClientDTO> AddExternalClients()
        => new Dictionary<string, HttpClientDTO>
            {
                {
                    "Microservices2",
                    new HttpClientDTO
                    {
                        HttpClient = microservices2Client.GetHttpClient(),
                        Endpoint = "/contract"
                    }
                }
            };
}
