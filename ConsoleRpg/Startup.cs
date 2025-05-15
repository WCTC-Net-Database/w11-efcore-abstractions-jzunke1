using Castle.Core.Configuration;
using ConsoleRpg.Helpers;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Helpers;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ConsoleRpgShared.Managers;

namespace ConsoleRpg;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        // Build configuration
        var configuration = ConfigurationHelper.GetConfiguration(Directory.GetCurrentDirectory());

        services.AddTransient<IRoomFactory, RoomFactory>();

        // Configure logging (console only)
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();
            // loggingBuilder.AddDebug(); // Optional: for debug output
        });

        // Register DbContext with dependency injection
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<GameContext>(options =>
        {
            ConfigurationHelper.ConfigureDbContextOptions(options, connectionString);
        });

        // Register your services
        services.AddTransient<GameEngine>();
        services.AddTransient<MenuManager>();
        services.AddSingleton<OutputManager>();
    }
}
