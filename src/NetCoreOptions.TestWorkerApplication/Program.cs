using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace NetCoreOptions.TestWorkerApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            //add and initialize the appsettings.json properties in the OptionsConfig<>
            services.AddOptionsConfig<TestWorkerApplicationOptions>(configuration, true);

            StartApplication();
        }

        static void StartApplication()
        {
            try
            {
                Console.WriteLine(OptionsConfig<TestWorkerApplicationOptions>.Options.ApplicationName);
                Console.WriteLine();
                Console.WriteLine("Initializing count:");
                int count = 0;
                while (true)
                {
                    Thread.Sleep(GetSleepTime());
                    count++;

                    int periodValue = OptionsConfig<TestWorkerApplicationOptions>.Options.TextConfiguration.PeriodValue * count;
                    string periodUnit = $"{OptionsConfig<TestWorkerApplicationOptions>.Options.TextConfiguration.PeriodUnit}{(periodValue > 1 ? "s" : "")}";

                    string countMessage = $"{periodValue} {periodUnit}";
                    Console.WriteLine(countMessage);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        static int GetSleepTime()
        {
            return OptionsConfig<TestWorkerApplicationOptions>.Options.TextConfiguration.PeriodUnit switch
            {
                "millisecond" => OptionsConfig<TestWorkerApplicationOptions>.Options.TextConfiguration.PeriodValue * 1,
                "second" => OptionsConfig<TestWorkerApplicationOptions>.Options.TextConfiguration.PeriodValue * 1000,
                "minute" => OptionsConfig<TestWorkerApplicationOptions>.Options.TextConfiguration.PeriodValue * 1000 * 60,
                "hour" => OptionsConfig<TestWorkerApplicationOptions>.Options.TextConfiguration.PeriodValue * 1000 * 60 * 60,
                _ => throw new Exception("Period Unit not supported. Please update appsettings.json file. Supported period units: millisecond, second, minute and hour."),
            };
        }
    }
}
