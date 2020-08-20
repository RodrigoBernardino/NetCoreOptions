using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace NetCoreOptions.Domain.Extensions
{
    public static class ConfiguraitonExtensions
    {
        public static T SafeGet<T>(this IConfiguration configuration)
        {
            string typeName = typeof(T).Name;

            if (configuration.GetChildren().Any(item => item.Key == typeName))
                configuration = configuration.GetSection(typeName);

            T model = configuration.Get<T>();

            if (model == null)
                throw new InvalidOperationException(
                    $"Configuration item for type {typeof(T).FullName} not found.");

            return model;
        }

        public static IConfiguration SafeGetConfigSection<T>(this IConfiguration configuration)
        {
            string typeName = typeof(T).Name;

            if (configuration.GetChildren().Any(item => item.Key == typeName))
                configuration = configuration.GetSection(typeName);

            T model = configuration.Get<T>();

            if (model == null)
                throw new InvalidOperationException(
                    $"Configuration item for type {typeof(T).FullName} not found.");

            return configuration;
        }
    }
}
