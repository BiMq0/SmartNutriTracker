using System.Reflection;

namespace SmartNutriTracker.Back.Handlers;

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

    public static IServiceCollection AddScopedMappers(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        var classes = types.Where(t => t.IsClass && t.Name.EndsWith("Mapper"));
        foreach (var @class in classes)
        {
            services.AddScoped(@class);

        }
        return services;
    }

    public static IServiceCollection AddAllScopes(this IServiceCollection services)
    {
        services.AddScopedServices();
        services.AddScopedMappers();
        return services;
    }
}
