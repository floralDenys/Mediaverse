using System;
using Mediaverse.Domain.Authentication.Entities;
using Mediaverse.Infrastructure.ContentSearch.Repositories.Dtos;
using Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mediaverse.Infrastructure.Common.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
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
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Type).HasConversion<int>();

            modelBuilder.Entity<RoomDto>().ToTable("Rooms");
            modelBuilder.Entity<RoomDto>().Property(r => r.Id).ValueGeneratedNever();
            modelBuilder.Entity<RoomDto>().HasKey(r => r.Id);
            modelBuilder.Entity<RoomDto>()
                .HasMany(r => r.Viewers)
                .WithOne(v => v.Room);
            modelBuilder.Entity<RoomDto>()
                .HasOne(r => r.CurrentContent)
                .WithOne(cc => cc.Room);
            modelBuilder.Entity<RoomDto>().Property(r => r.Type).HasConversion<int>();

            modelBuilder.Entity<CurrentContentDto>().ToTable("CurrentRoomsContent");
            modelBuilder.Entity<CurrentContentDto>().HasKey(cc => cc.RoomId);
            modelBuilder.Entity<CurrentContentDto>().Property(cc => cc.RoomId).ValueGeneratedNever();
            modelBuilder.Entity<CurrentContentDto>()
                .HasOne(cc => cc.Room)
                .WithOne(r => r.CurrentContent);
            modelBuilder.Entity<CurrentContentDto>().Property(cc => cc.Source).HasConversion<int>();
            modelBuilder.Entity<CurrentContentDto>().Property(cc => cc.Type).HasConversion<int>();
            
            modelBuilder.Entity<ViewerDto>().ToTable("RoomViewers");
            modelBuilder.Entity<ViewerDto>().ToTable("RoomViewers")
                .HasKey(v => new {v.Id, v.RoomId});
            modelBuilder.Entity<ViewerDto>().Property(v => v.Id)
                .ValueGeneratedNever();
            modelBuilder.Entity<ViewerDto>()
                .HasOne(v => v.Room)
                .WithMany(r => r.Viewers);

            modelBuilder.Entity<PlaylistDto>().ToTable("Playlists");
            modelBuilder.Entity<PlaylistDto>().Property(p => p.Id).ValueGeneratedNever();
            modelBuilder.Entity<PlaylistDto>().HasKey(p => p.Id);
            modelBuilder.Entity<PlaylistDto>()
                .HasMany(p => p.PlaylistItems)
                .WithOne(pi => pi.Playlist);

            modelBuilder.Entity<PlaylistItemDto>().ToTable("PlaylistItems");
            modelBuilder.Entity<PlaylistItemDto>()
                .HasOne(pi => pi.Playlist)
                .WithMany(p => p.PlaylistItems);
            modelBuilder.Entity<PlaylistItemDto>().HasKey(pi => 
                new {pi.ExternalId, pi.ContentSource, pi.ContentType, pi.PlaylistId});

            modelBuilder.Entity<ContentDto>().ToTable("CachedContent");
            modelBuilder.Entity<ContentDto>().HasKey(c =>
                new {c.ExternalId, c.ContentSource, c.ContentType});
            modelBuilder.Entity<ContentDto>().Property(c => c.ContentSource).HasConversion<int>();
            modelBuilder.Entity<ContentDto>().Property(c => c.ContentType).HasConversion<int>();
        }
    }
}