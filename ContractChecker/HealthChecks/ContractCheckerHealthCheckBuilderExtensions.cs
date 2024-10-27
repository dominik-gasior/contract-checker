using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ContractChecker.HealthChecks;

public static class ContractCheckerHealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddContractCheckerHealthCheck(
        this IHealthChecksBuilder builder,
        string name,
        ContractCheckerConfiguration externalClientConfigurations,
        HealthStatus? failureStatus = default,
        IEnumerable<string> tags = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name,
            sp => new ContractCheckerHealthCheck(
                sp.GetRequiredService<IHttpClientFactory>(),
                externalClientConfigurations),
            failureStatus,
            tags));
    }
}