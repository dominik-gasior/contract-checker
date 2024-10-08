using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ContractChecker;

public abstract class ContractCheckerHealthCheck : IHealthCheck
{
    /// <summary>
    /// ExternalClients collection which it is needed to fetch http clients which you can check.
    /// <para>Key -> Service name</para>
    /// <para>Value -> HttpClient</para>
    /// </summary>
    private Dictionary<string, HttpClientDTO> ExternalClients;
    private readonly Dictionary<string, IContract?> contracts;

    public ContractCheckerHealthCheck()
    {
        contracts = AssemblyExtensions.GetContracts() ?? new Dictionary<string, IContract?>();
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        ExternalClients = AddExternalClients();

        if (contracts is null || contracts.Count == 0)
            return HealthCheckResult.Unhealthy("Contracts are not loaded");

        foreach (var client in ExternalClients)
        {
            foreach (var contract in contracts)
            {
                var contractName = contract.Key;
                var response = await GetResponse(client.Value.HttpClient, client.Value.Endpoint, contractName);

                if (response == null)
                    return HealthCheckResult.Unhealthy($"Client {client.Key} is not healthy or not reachable");

                var contractDTO = ContractExtensions.GetDTO(contractName);

                if (!JsonExtensions.CompareJson(contractDTO, response))
                    return HealthCheckResult.Unhealthy($"Contract {contract.GetType().Name} is not valid for {client.Key}");
            }
        }

        return HealthCheckResult.Healthy();
    }

    protected abstract Dictionary<string, HttpClientDTO> AddExternalClients();

    private async Task<string?> GetResponse(HttpClient httpClient, string endpoint, string contractName)
    {
        try
        {
            return await httpClient.GetStringAsync($"{endpoint}?className={contractName}");
        }
        catch (Exception)
        {
            return null;
        }
    }
}