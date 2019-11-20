using Microsoft.Extensions.Logging;
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
        public MyClass(ILogger<MyClass> logger)
        {
            _logger = logger;
        }

        public void DoWork()
        {
            _logger.LogWarning("Warn Start DoWork");

        }

        public async Task DoWorkAsync()
        {
            _logger.LogInformation("Start DoWorkAsync");
            string[] lines =  await File.ReadAllLinesAsync("appsettings.json");
            _logger.LogInformation("Stop DoWorkAsync");
        }


    }
}
