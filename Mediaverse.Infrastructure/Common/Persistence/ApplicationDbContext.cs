using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mediaverse.Infrastructure.Common.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RoomDto> Rooms { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-9G78RAB\\SQLEXPRESS;database=mediaverse;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<User>().HasKey(u => u.Id);

            modelBuilder.Entity<RoomDto>().ToTable("Rooms");
            modelBuilder.Entity<RoomDto>().HasKey(r => r.Id);
            modelBuilder.Entity<RoomDto>();
            modelBuilder.Entity<RoomDto>()
                .HasMany(r => r.Viewers);
        }
    }
}