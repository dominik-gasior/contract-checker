using ContractChecker;
using Microservices1;
using Microservices1.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<Microservices2Client>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8001");
});

builder.Services.AddHealthChecks()
    .AddCheck<CustomContractCheckerHealthCheck>("ContractChecker");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test Web Application");
    c.RoutePrefix = "";
});


app.UseHttpsRedirection();

app.MapHealthChecks("/health");
app.AddContractCheckerEndpoint("/contract");

app.Run();