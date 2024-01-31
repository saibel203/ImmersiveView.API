using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ImmersiveView.Identity.Seeds;

public class SeedUsersDbContext(ImmersiveViewUsersDbContext usersDbContext)
{
    private readonly ILogger _logger = Log.ForContext<SeedUsersDbContext>();
    
    public async Task InitializeUsersDatabaseAsync()
    {
        try
        {
            if (usersDbContext.Database.IsSqlServer())
                await usersDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while trying to initialize the database.");
            throw;
        }
    }

    public async Task SeedContextDataAsync()
    {
        try
        {
            await TrySeedUsersDataAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while trying to seed the database.");
            throw;
        }
    }

    private async Task TrySeedUsersDataAsync()
    {
        
    }
}