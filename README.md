# NetCoreOptions
This is a lightweight project for better integration with .NET Core configuration and appsettings.json properties.

The purpose of this project is to facilitate the use of the .NET Core configuration without referring to the IOptions directly and without referring to the configuration properties by string name throughout the code. This is best for code scalability and maintenance, because it makes it easier to change configuration properties during the development phase and even in production phase.

## Instalation
This project will be added to NuGet gallery soon. For now, download this solution and add its projects to your target solution.

## How to Use
The use of this project is very simple. The first thing to do is to create a c# class that represents the appsettings.json file. This can be done easily using the website http://jsonutils.com/. For example, a appsettings.json file like this:

```JSON
{
  "ApplicationName": "NetCoreOptions Counting Application",
  "TextConfiguration": {
    "PeriodValue": 1,
    "PeriodUnit":  "second"
  }
}
```

Will be represented by this c# class:

```C#
public class TestWorkerApplicationOptions : IAppSettingsOptions
{
    public string ApplicationName { get; set; }
    public TextConfiguration TextConfiguration { get; set; }
}

public class TextConfiguration
{
    public int PeriodValue { get; set; }
    public string PeriodUnit { get; set; }
}
```

This class must inherit the ``` IAppSettingsOptions ``` interface. This interface has nothing to implement, it is just for better design and to limit the classes that can be used as configuration options.

The next step is to configure the ``` OptionsConfig ``` class in the ``` ServiceCollection ``` at the project startup. In a console app:

```C#
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
```

The ``` AddOptionsConfig<> ``` method accepts two parameters. The first one is the ``` IConfiguration ``` of your project, which is a key/value set that represents the configuration properties. The second one is a boolean flag that says whether changes in the appsettings.json file are ready to use with or without a project restart. If set to **true** the changes will be captured by the ``` OptionsConfig<> ``` class on the next request. If set to **false**, the changes will be captured only after a project restart.

The configuration properties can now be used anywhere just using the generic ``` OptionsConfig<> ``` static class with the ``` IAppSettingsOptions ``` class that represents the appsettings.json file as the type:

```C#
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
```

These sample codes are from a test console app that is also in this repository solution. 
