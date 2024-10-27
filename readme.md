# ContractChecker

A library helps to check contracts between microservices.

### Goal

Removing the problem caused by the need to update contracts between services, and we do not always remember to update contracts and they may become inconsistent between services.

### Description

The application works on the system of health checks. Health check communicates with services that are connected to the main service, and then retrieves contract structures from them to ensure that the contract structure is consistent. It is enough that one contract is not consistent and the health check will return information that the application status is unhealthy and log it in the logger as critical.

Once health check checks only one service.

### Example configurations

```csharp
// add microservices2 http client
builder.Services.AddHttpClient<Microservices2Client>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8001");
});

// add contract checker health check
builder.Services.AddHealthChecks()
    .AddContractCheckerHealthCheck("ContractChecker", new ContractCheckerConfiguration
    {
        ServiceName = "Microservices2",
        HttpClient = typeof(Microservices2Client),
        Endpoint = "/contract",
        ContractDTOs = [
            new ContractDTO(typeof(CityDTO), "CityDTO"),
            new ContractDTO(typeof(CompanyDTO), "CompanyDTO")
        ]
    });

// Add contract checker endpoint
app.AddContractCheckerEndpoint("/contract");
```