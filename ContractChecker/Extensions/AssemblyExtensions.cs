using System.Collections;
using System.Reflection;


namespace ContractChecker;

internal static class AssemblyExtensions
{
    internal static object? GetObjectProperties(string className)
    {
        try
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

            var type = assembly.GetTypes()
            .FirstOrDefault(t => t.Name == className && typeof(IContract).IsAssignableFrom(t));

            if (type == null)
                throw new Exception($"Class '{className}' not found or does not implement IContract.");

            var obj = Activator.CreateInstance(type);
            InitializeProperties(obj);

            return obj;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating object: {ex.Message}");
        }
    }

    internal static Dictionary<string, IContract?>? GetContracts()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

        var contracts = assembly.GetTypes()
            .Where(t => typeof(IContract).IsAssignableFrom(t) && !t.IsInterface)
            .Select(t => new KeyValuePair<string, IContract?>(t.Name, (IContract?)Activator.CreateInstance(t)))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        return contracts != null && contracts.Count != 0 ? contracts : null;
    }

    internal static void InitializeProperties(object? obj)
    {
        if (obj == null)
            return;

        var properties = obj.GetType().GetProperties();

        foreach (var property in properties)
        {
            SetPropertyValue(property, obj);
        }
    }

    internal static void SetPropertyValue(PropertyInfo? property, object obj)
    {
        if (property.CanWrite)
        {
            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                var collection = Activator.CreateInstance(property.PropertyType);
                var addMethod = property.PropertyType.GetMethod("Add");

                if (addMethod != null)
                {
                    var itemType = property.PropertyType.GetGenericArguments()[0];
                    var item = Activator.CreateInstance(itemType);
                    addMethod.Invoke(collection, new[] { item });
                }
                property.SetValue(obj, collection);
            }
            else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                var nestedInstance = Activator.CreateInstance(property.PropertyType);
                property.SetValue(obj, nestedInstance);
            }
        }
    }
}
