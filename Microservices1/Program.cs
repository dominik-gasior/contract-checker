using ContractChecker;
using ContractChecker.HealthChecks;
using Microservices1.Clients;
using Microservices1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test Web Application");
    c.RoutePrefix = "";
});

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

// Add contract checker endpoint
app.AddContractCheckerEndpoint("/contract");

app.Run();