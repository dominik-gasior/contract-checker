using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ContractChecker;

public static class Checker
{
    public static void AddContractChecker(this IEndpointRouteBuilder endpoints, string route)
    {
        endpoints.MapGet(route, async context =>
        {
            var className = context.Request.Query["className"].ToString();

            try
            {
                var json = GetDTO(className);

                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync($"Class {className} not found");
                return;
            }
        });
    }

    private static string? GetDTO(string className)
    {
        var properties = AssemblyExtensions.GetObjectProperties(className);
        var json = JsonSerializer.Serialize(properties, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return json;
    }

}