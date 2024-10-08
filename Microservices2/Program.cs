using ContractChecker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test Web Application");
    c.RoutePrefix = "";
});

app.UseHttpsRedirection();
app.AddContractCheckerEndpoint("/contract");

app.Run();