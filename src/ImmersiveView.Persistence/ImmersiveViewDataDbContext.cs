using Microsoft.EntityFrameworkCore;

namespace ImmersiveView.Persistence;

public class ImmersiveViewDataDbContext(DbContextOptions<ImmersiveViewDataDbContext> options) : DbContext(options);