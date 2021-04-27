using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Domain.JointContentConsumption.Entities;
using Mediaverse.Infrastructure.ContentSearch.Repositories.Dtos;
using Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mediaverse.Infrastructure.Common.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RoomDto> Rooms { get; set; }
        public DbSet<PlaylistDto> Playlists { get; set; }
        public DbSet<ContentDto> CachedContent { get; set; }
        
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
            modelBuilder.Entity<User>().Property(u => u.Type).HasConversion<int>();

            modelBuilder.Entity<RoomDto>().ToTable("Rooms");
            modelBuilder.Entity<RoomDto>().HasKey(r => r.Id);
            modelBuilder.Entity<RoomDto>().HasMany<ViewerDto>();
            modelBuilder.Entity<RoomDto>().Property(r => r.Type).HasConversion<int>();

            modelBuilder.Entity<PlaylistDto>().ToTable("Playlists");
            modelBuilder.Entity<PlaylistDto>().HasKey(p => p.Id);
            modelBuilder.Entity<PlaylistDto>()
                .HasMany(p => p.PlaylistItems)
                .WithOne(pi => pi.Playlist);

            modelBuilder.Entity<PlaylistItemDto>().ToTable("PlaylistItems");
            modelBuilder.Entity<PlaylistItemDto>()
                .HasOne(pi => pi.Playlist)
                .WithMany(p => p.PlaylistItems);
            modelBuilder.Entity<PlaylistItemDto>().HasKey(pi => 
                new {pi.ExternalId, pi.ContentSource, pi.ContentType});

            modelBuilder.Entity<ContentDto>().ToTable("CachedContent");
            modelBuilder.Entity<ContentDto>().HasKey(c =>
                new {c.ExternalId, c.ContentSource, c.ContentType});
            modelBuilder.Entity<ContentDto>().Property(c => c.ContentSource).HasConversion<int>();
            modelBuilder.Entity<ContentDto>().Property(c => c.ContentType).HasConversion<int>();
        }
    }
}