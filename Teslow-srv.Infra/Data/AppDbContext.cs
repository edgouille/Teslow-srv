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
        public DbSet<TeamMembership> TeamMemberships => Set<TeamMembership>();
        public DbSet<GameTable> GameTables => Set<GameTable>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<GameTableAssignment> GameTableAssignments => Set<GameTableAssignment>();
        public DbSet<GameTeam> GameTeams => Set<GameTeam>();
        public DbSet<TeamPlayer> TeamPlayers => Set<TeamPlayer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UserName).HasMaxLength(100);
                entity.Property(u => u.DisplayName).HasMaxLength(100);
                entity.Property(u => u.CanonicalName).HasMaxLength(100);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.GameId);
                entity.Property(g => g.GameId).HasMaxLength(50);
                entity.Property(g => g.ScoreRed).IsRequired();
                entity.Property(g => g.ScoreBleu).IsRequired();
                entity.Property(g => g.GameDuration).IsRequired();
                entity.Property(g => g.GameDate).IsRequired();
            });

            modelBuilder.Entity<TeamMembership>(entity =>
            {
                entity.HasKey(t => t.TeamId);
                entity.Property(t => t.TeamColor).HasMaxLength(50);
            });

            modelBuilder.Entity<GameTable>(entity =>
            {
                entity.HasKey(t => t.GameTableId);
                entity.Property(t => t.GameTableId).HasMaxLength(50);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.ReservationId);
                entity.Property(r => r.Status).HasMaxLength(50);
                entity.Property(r => r.GameId).HasMaxLength(50);

                entity.HasOne(r => r.Game)
                    .WithOne(g => g.Reservation)
                    .HasForeignKey<Reservation>(r => r.GameId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<GameTableAssignment>(entity =>
            {
                entity.HasKey(gt => new { gt.GameId, gt.GameTableId });

                entity.Property(gt => gt.GameId).HasMaxLength(50);
                entity.Property(gt => gt.GameTableId).HasMaxLength(50);

                entity.HasOne(gt => gt.Game)
                    .WithMany(g => g.GameTables)
                    .HasForeignKey(gt => gt.GameId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(gt => gt.GameTable)
                    .WithMany(t => t.GameAssignments)
                    .HasForeignKey(gt => gt.GameTableId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<GameTeam>(entity =>
            {
                entity.HasKey(gt => new { gt.GameId, gt.TeamId });

                entity.Property(gt => gt.GameId).HasMaxLength(50);

                entity.HasOne(gt => gt.Game)
                    .WithMany(g => g.GameTeams)
                    .HasForeignKey(gt => gt.GameId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(gt => gt.Team)
                    .WithMany(t => t.GameTeams)
                    .HasForeignKey(gt => gt.TeamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TeamPlayer>(entity =>
            {
                entity.HasKey(tp => new { tp.UserId, tp.TeamId });

                entity.HasOne(tp => tp.User)
                    .WithMany(u => u.TeamPlayers)
                    .HasForeignKey(tp => tp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tp => tp.Team)
                    .WithMany(t => t.TeamPlayers)
                    .HasForeignKey(tp => tp.TeamId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
