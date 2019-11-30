using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyBaseConsoleApp
{
    public class MyClass
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _conf;
        public MyClass(ILogger<MyClass> logger, IConfiguration conf)
        {
            _logger = logger;
            _conf = conf;
        }

        public void DoWork()
        {
            var configKey1 = _conf.GetSection("MyClass").GetValue<string>("ConfigKey1");
            _logger.LogWarning($"Warn Start DoWork, configKey1: {configKey1}");

        }

        public async Task DoWorkAsync()
        {
            _logger.LogInformation("Start DoWorkAsync");
            string[] lines =  await File.ReadAllLinesAsync("appsettings.json");
            _logger.LogInformation("Stop DoWorkAsync");
        }


    }
}
