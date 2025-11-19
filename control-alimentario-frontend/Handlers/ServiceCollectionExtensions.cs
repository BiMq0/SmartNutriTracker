using System.Reflection;

namespace control_alimentario_backend.Handlers;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddScopedServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        var interfaces = types.Where(t => t.IsInterface && t.Name.EndsWith("Service"));
        foreach (var itf in interfaces)
        {
            var implementation = types.FirstOrDefault(t => itf.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
            if (implementation != null)
            {
                services.AddScoped(itf, implementation);
            }
        }
        return services;
    }
}
