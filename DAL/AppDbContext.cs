using Microsoft.EntityFrameworkCore;
using DAL.Models;

namespace DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public AppDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql("server=localhost;database=MyWeightPal;user=root;password=12345678",
                new MySqlServerVersion(new Version(8, 0, 32)));
        }
    }

    public DbSet<GewichtModel> Gewichten { get; set; }
    public DbSet<DoelGewichtModel> DoelGewichten { get; set; }
}