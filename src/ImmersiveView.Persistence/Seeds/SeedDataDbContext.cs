using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ImmersiveView.Persistence.Seeds;

public class SeedDataDbContext(ImmersiveViewDataDbContext dataDbContext)
{
    private readonly ILogger _logger = Log.ForContext<SeedDataDbContext>();

    public async Task InitializeUsersDatabaseAsync()
    {
        try
        {
            if (dataDbContext.Database.IsSqlServer())
                await dataDbContext.Database.MigrateAsync();
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