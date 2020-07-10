using System;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // "using" makes sure everything inside the statement is disposed as soon as the method is finished
            using(var scope = host.Services.CreateScope()) //
            {
                // gets all our services 
                var services = scope.ServiceProvider;
                // LoggerFactory?
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreContext>();
                    // applies any pending migrations to the context to the database or creates the database if it exists 
                    // as soon as the application starts 
                    await context.Database.MigrateAsync();
                    // seeds the database 
                    await StoreContextSeed.SeedAsync(context, loggerFactory);
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error accured during Migration"); 
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
