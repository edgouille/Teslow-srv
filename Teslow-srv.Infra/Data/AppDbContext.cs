using Microsoft.EntityFrameworkCore;
using Teslow_srv.Domain.Entities;

namespace Teslow_srv.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Game> Games => Set<Game>();
        
    }
}