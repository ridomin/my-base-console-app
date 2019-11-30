using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
            IConfiguration configuration = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json")
                                            .Build();

            var serviceCollection = new ServiceCollection();
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
