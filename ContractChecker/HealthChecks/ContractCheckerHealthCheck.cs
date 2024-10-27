using ContractChecker.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ContractChecker.HealthChecks;

internal sealed class ContractCheckerHealthCheck : IHealthCheck
{
    /// <summary>
    /// ExternalClients collection which it is needed to fetch http clients which you can check.
    /// <para>Key -> Service name</para>
    /// <para>Value -> HttpClient</para>
    /// </summary>
    private readonly ContractCheckerConfiguration _clientConfigurations;
    private readonly HttpClient _httpClient;

    public ContractCheckerHealthCheck(
        IHttpClientFactory httpClientFactory,
        ContractCheckerConfiguration clientConfigurations)
    {
        _clientConfigurations = clientConfigurations;
        _httpClient = httpClientFactory.CreateClient(_clientConfigurations.HttpClient.Name);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var contracts = _clientConfigurations.ContractDTOs.ToList();
        
        if (contracts.Count == 0)
            return HealthCheckResult.Unhealthy("Contracts are not loaded");
        
        var serviceName = _clientConfigurations.ServiceName;
        
        foreach (var contract in contracts)
        {
            var destinationContractDTO = await GetResponse(
                _httpClient,
                _clientConfigurations.Endpoint,
                contract.DestinationName);

            if (destinationContractDTO == null)
                return HealthCheckResult.Unhealthy($"Client {serviceName} is not healthy or not reachable");

            var sourceContractDTO = ContractExtensions.GetDTO(contract.SourceName.Name);

            if (!JsonExtensions.CompareJson(sourceContractDTO, destinationContractDTO))
                return HealthCheckResult.Unhealthy($"Contract {sourceContractDTO.GetType().Name} is not valid for {serviceName}");
        }

        return HealthCheckResult.Healthy();
    }
    
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