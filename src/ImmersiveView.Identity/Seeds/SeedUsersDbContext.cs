using Microsoft.EntityFrameworkCore;

namespace ImmersiveView.Identity.Seeds;

public class SeedUsersDbContext(ImmersiveViewUsersDbContext usersDbContext)
{
    public async Task InitializeUsersDatabaseAsync()
    {
        try
        {
            if (usersDbContext.Database.IsSqlServer())
                await usersDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
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
        }
    }

    private async Task TrySeedUsersDataAsync()
    {
    }
}