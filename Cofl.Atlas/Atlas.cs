using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;
using System.IO;

namespace Cofl.Atlas
{
    sealed internal class Atlas
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Environment.CurrentDirectory)
                                    .AddJsonFile("./.atlas")
                                    .Build();
            var services = GetServices();
            using(var scope = services.CreateScope())
            {
                var helper = scope.ServiceProvider.GetRequiredService<object>();
            }
            Console.WriteLine("Hello World!");
        }

        private static IServiceScopeFactory GetServices(AtlasConfiguration configuration = null) {
            configuration = configuration ?? AtlasConfiguration.DefaultConfiguration;
            var fileProvider = new PhysicalFileProvider(configuration.FileProviderPath);

            return new ServiceCollection()
                .AddSingleton<IHostingEnvironment>(new HostingEnvironment
                {
                    ApplicationName = configuration.ApplicationName,
                    WebRootFileProvider = fileProvider
                })
                .Configure<RazorViewEngineOptions>(options => {
                    options.FileProviders.Clear();
                    options.FileProviders.Add(fileProvider);
                })
                .AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>()
                .AddSingleton<DiagnosticSource>(new DiagnosticListener("Microsoft.AspNetCore"))
                .AddLogging()
                .AddMvc()
                .AddTransient<object>()
                .BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>();
        }
    }

    internal static class Extensions
    {
        internal static IServiceCollection AddMvc(this IServiceCollection collection)
        {
            collection.AddMvcCore();
            return collection;
        }
    }
}
