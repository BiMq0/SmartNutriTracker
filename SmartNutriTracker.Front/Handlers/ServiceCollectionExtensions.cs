using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SmartNutriTracker.Front.Handlers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes().Where(t => t.IsClass && t.Name.EndsWith("Service")).ToList();
            foreach (var type in types)
            {
                Console.WriteLine($"Registering service: {type.Name}");
                services.AddScoped(type);
            }
            return services;
        }
    }
}