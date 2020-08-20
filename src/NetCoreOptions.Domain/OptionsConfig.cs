using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace NetCoreOptions
{
    public static class OptionsConfig<T>
        where T : class, IAppSettingsOptions, new()
    {
        private static IOptions<T> _options;
        private static IOptionsMonitor<T> _optionsMonitor;

        private static bool? _allowSettingsRealTimeUpdate = null;

        /// <summary>
        /// Provides the configuration object that represents the appsettings.json file.
        /// </summary>
        public static T Options
        {
            get
            {
                if (_allowSettingsRealTimeUpdate.Value)
                    return GetMonitorOptions();
                else
                    return GetOptions();
            }
        }

        /// <summary>
        /// Initialize the OptionsConfig with the class that represents the appsettings.json file.
        /// </summary>
        /// <param name="serviceProvider">Services Collection.</param>
        /// <param name="allowSettingsRealTimeUpdate">Define if changes in the appsettings.json will be catch at runtime (without application reset) or not. Default value is false.</param>
        public static void Initialize(IServiceProvider serviceProvider, bool allowSettingsRealTimeUpdate = false)
        {
            if (_allowSettingsRealTimeUpdate.HasValue)
                throw new InvalidOperationException(
                            "The OptionConfig already was configured. It is not allowed to configure this twice.");

            _allowSettingsRealTimeUpdate = allowSettingsRealTimeUpdate;

            if (allowSettingsRealTimeUpdate)
                InitializeRealTimeUpdate(serviceProvider);
            else
                InitializeSingleton(serviceProvider);
        }

        private static void InitializeRealTimeUpdate(IServiceProvider serviceProvider)
        {
            if (_optionsMonitor == null)
                _optionsMonitor = serviceProvider.GetRequiredService<IOptionsMonitor<T>>();
        }

        private static void InitializeSingleton(IServiceProvider serviceProvider)
        {
            if (_options == null)
                _options = serviceProvider.GetRequiredService<IOptions<T>>();
        }

        private static T GetMonitorOptions()
        {
            if (_optionsMonitor == null)
                throw new InvalidOperationException(
                    "The OptionConfig was not configured correctly in the startup.");

            return _optionsMonitor.CurrentValue;
        }

        private static T GetOptions()
        {
            if (_options == null)
                throw new InvalidOperationException(
                    "The OptionConfig was not configured correctly in the startup.");

            return _options.Value;
        }
    }
}
