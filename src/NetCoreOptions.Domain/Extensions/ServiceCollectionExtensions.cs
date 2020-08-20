using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreOptions.Domain.Extensions;
using System;

namespace NetCoreOptions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add and initialize the OptionsConfig<> with the class that represents the appsettings.json file.
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        /// <param name="allowSettingsRealTimeUpdate">Define if changes in the appsettings.json will be catch at runtime (without application reset) or not. Default value is false.</param>
        public static void AddOptionsConfig<T>(this IServiceCollection services,
            IConfiguration configuration, bool allowSettingsRealTimeUpdate = false)
            where T : class, IAppSettingsOptions, new()
        {
            IConfiguration appSettingsConfig = configuration.SafeGetConfigSection<T>();
            services.Configure<T>(appSettingsConfig);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            OptionsConfig<T>.Initialize(serviceProvider, allowSettingsRealTimeUpdate);
        }
    }
}
