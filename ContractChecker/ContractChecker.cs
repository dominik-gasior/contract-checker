using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ContractChecker;

public static class Checker
{
    public static IEndpointRouteBuilder AddContractCheckerEndpoint(
        this IEndpointRouteBuilder endpoints,
        string route)
    {
        endpoints.MapGet(route, async context =>
        {
            var className = context.Request.Query["className"].ToString();

            try
            {
                var json = ContractExtensions.GetDTO(className);

                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync($"Class {className} not found.");
                // TODO: Log the exception
                return;
            }
        });

        return endpoints;
    }
}