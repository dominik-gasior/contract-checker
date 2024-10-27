namespace ContractChecker.Extensions;

internal static class ContractExtensions
{
    public static string? GetDTO(string className)
    {
        var obj = AssemblyExtensions.GetObjectProperties(className);
        return JsonExtensions.Serialize(obj);
    }
}
