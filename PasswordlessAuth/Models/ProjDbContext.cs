using Microsoft.EntityFrameworkCore;

namespace PasswordlessAuth.Models;

public class ProjDbContext : DbContext
{
    public ProjDbContext(DbContextOptions<ProjDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}