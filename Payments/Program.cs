﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;

namespace Orders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseCloudFoundryHosting()
                .AddCloudFoundry()
                .ConfigureLogging((builderContext, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                    loggingBuilder.AddDynamicConsole();
                })
                .UseStartup<Startup>()
                .Build();
    }
}
