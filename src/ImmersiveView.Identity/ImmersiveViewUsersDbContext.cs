using ImmersiveView.Identity.Configurations.Additional;
using ImmersiveView.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImmersiveView.Identity;

public class ImmersiveViewUsersDbContext(DbContextOptions<ImmersiveViewUsersDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationUser).Assembly);

        // Additional configuration
        builder.ApplyConfigurationsFromAssembly(typeof(GenderEntityConfiguration).Assembly);
    }
}