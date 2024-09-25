using System.Reflection;

namespace ContractChecker;

internal static class AssemblyExtensions
{
    internal static object GetObjectProperties(string className)
    {
        try
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

            var type = assembly.GetTypes()
            .FirstOrDefault(t => t.Name == className && typeof(IContract).IsAssignableFrom(t));

            if (type == null)
                throw new Exception($"Class '{className}' not found or does not implement ITest.");

            var instance = Activator.CreateInstance(type);

            var properties = type.GetProperties()
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(instance)?.ToString() ?? "null");

            return properties;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating object: {ex.Message}");
        }
    }
}
