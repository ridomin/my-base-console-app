using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace MyBaseConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("starting");
            ServiceProvider serviceProvider = ConfigureServices();

            var myClass = serviceProvider.GetService<MyClass>();

            myClass.DoWork();
            await myClass.DoWorkAsync();
            //the logger by adding a parameter (ILogger<MyClass>)
            var logger = serviceProvider.GetService<ILogger<Program>>();

            logger.LogInformation("Log in Program.cs");
            logger.LogWarning("Warning");
            logger.LogError("ERRROOORR");

            Console.ReadLine();
        }

        private static ServiceProvider ConfigureServices()
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || 
                devEnvironmentVariable.ToLower(CultureInfo.CurrentCulture) == "development";
            
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
                                            

            if (isDevelopment) //only add secrets in development
            {
                builder.AddUserSecrets<MyClass>();
            }

            IConfiguration configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure<MyClass>(configuration.GetSection("MyClass"));
            serviceCollection.AddLogging(configure => configure.AddConsole(ops => ops.TimestampFormat = "hh:mm:ss")
                .AddConfiguration(configuration.GetSection("Logging")))
            .AddSingleton(typeof(IConfiguration), configuration)
            .AddTransient<MyClass>();

            //LogLevel minLevel = Enum.Parse<LogLevel>(configuration["Logging:Console:LogLevel:Default"].ToString());
            //serviceCollection.Configure<LoggerFilterOptions>(options => options.MinLevel = minLevel);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

    }
}
