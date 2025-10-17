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
        public DbSet<GameTeam> GameTeams => Set<GameTeam>();
        public DbSet<TeamPlayer> TeamPlayers => Set<TeamPlayer>();
        public DbSet<TeamMembership> TeamMemberships => Set<TeamMembership>();
        public DbSet<GameTable> GameTables => Set<GameTable>();
        public DbSet<GameTableAssignment> GameTableAssignments => Set<GameTableAssignment>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UserName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Role).HasMaxLength(50);
                entity.Property(u => u.PasswordHash).HasMaxLength(512).IsRequired();
                entity.HasIndex(u => u.UserName).IsUnique();

                entity.HasMany(u => u.TeamPlayers)
                    .WithOne(tp => tp.User)
                    .HasForeignKey(tp => tp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.TeamMemberships)
                    .WithOne(tm => tm.User)
                    .HasForeignKey(tm => tm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Score1).IsRequired();
                entity.Property(g => g.Score2).IsRequired();
                entity.Property(g => g.DurationSeconds).IsRequired();
                entity.Property(g => g.Date).IsRequired();

                entity.HasMany(g => g.Teams)
                    .WithOne(gt => gt.Game)
                    .HasForeignKey(gt => gt.GameId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GameTeam>(entity =>
            {
                entity.HasKey(gt => gt.Id);
                entity.Property(gt => gt.TeamNumber).IsRequired();

                entity.HasMany(gt => gt.Players)
                    .WithOne(tp => tp.Team)
                    .HasForeignKey(tp => tp.TeamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TeamPlayer>(entity =>
            {
                entity.HasKey(tp => tp.Id);

                entity.HasOne(tp => tp.User)
                    .WithMany(u => u.TeamPlayers)
                    .HasForeignKey(tp => tp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tp => tp.Team)
                    .WithMany(gt => gt.Players)
                    .HasForeignKey(tp => tp.TeamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TeamMembership>(entity =>
            {
                entity.HasKey(tm => tm.Id);
                entity.HasIndex(tm => new { tm.TeamId, tm.UserId }).IsUnique();

                entity.HasOne(tm => tm.User)
                    .WithMany(u => u.TeamMemberships)
                    .HasForeignKey(tm => tm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GameTable>(entity =>
            {
                entity.HasKey(gt => gt.Id);
                entity.Property(gt => gt.Name).HasMaxLength(100).IsRequired();
                entity.Property(gt => gt.Location).HasMaxLength(200);

                entity.HasMany(gt => gt.Assignments)
                    .WithOne(a => a.GameTable)
                    .HasForeignKey(a => a.GameTableId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GameTableAssignment>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.Reservation)
                    .WithMany(r => r.TableAssignments)
                    .HasForeignKey(a => a.ReservationId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.GameTable)
                    .WithMany(gt => gt.Assignments)
                    .HasForeignKey(a => a.GameTableId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.StartUtc).IsRequired();
                entity.Property(r => r.DurationSeconds).IsRequired();
                entity.Property(r => r.Mode).IsRequired();
                entity.Property(r => r.CreatedAtUtc).IsRequired();
                entity.Property(r => r.RowVersion).IsRowVersion();
            });
        }
    }
}
