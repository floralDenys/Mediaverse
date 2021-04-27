﻿// <auto-generated />
using System;
using Mediaverse.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mediaverse.Infrastructure.Common.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210427072702_FixedPlaylistItemTable")]
    partial class FixedPlaylistItemTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mediaverse.Domain.Authentication.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nickname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Mediaverse.Infrastructure.ContentSearch.Repositories.Dtos.ContentDto", b =>
                {
                    b.Property<string>("ExternalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ContentSource")
                        .HasColumnType("int");

                    b.Property<int>("ContentType")
                        .HasColumnType("int");

                    b.Property<int>("ContentPlayerHeight")
                        .HasColumnType("int");

                    b.Property<string>("ContentPlayerHtml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContentPlayerWidth")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ThumbnailHeight")
                        .HasColumnType("bigint");

                    b.Property<string>("ThumbnailUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ThumbnailWidth")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExternalId", "ContentSource", "ContentType");

                    b.ToTable("CachedContent");
                });

            modelBuilder.Entity("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.PlaylistDto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CurrentlyPlayingContentIndex")
                        .HasColumnType("int");

                    b.Property<bool>("IsTemporary")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Playlists");
                });

            modelBuilder.Entity("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.PlaylistItemDto", b =>
                {
                    b.Property<string>("ExternalId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ContentSource")
                        .HasColumnType("int");

                    b.Property<int>("ContentType")
                        .HasColumnType("int");

                    b.Property<Guid?>("PlaylistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PlaylistItemIndex")
                        .HasColumnType("int");

                    b.HasKey("ExternalId", "ContentSource", "ContentType");

                    b.HasIndex("PlaylistId");

                    b.ToTable("PlaylistItems");
                });

            modelBuilder.Entity("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.RoomDto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ActivePlaylistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaxViewersQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.ViewerDto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RoomDtoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RoomDtoId1")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoomDtoId");

                    b.HasIndex("RoomDtoId1");

                    b.ToTable("ViewerDto");
                });

            modelBuilder.Entity("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.PlaylistItemDto", b =>
                {
                    b.HasOne("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.PlaylistDto", "Playlist")
                        .WithMany("PlaylistItems")
                        .HasForeignKey("PlaylistId");

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.ViewerDto", b =>
                {
                    b.HasOne("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.RoomDto", null)
                        .WithMany("Viewers")
                        .HasForeignKey("RoomDtoId");

                    b.HasOne("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.RoomDto", null)
                        .WithMany()
                        .HasForeignKey("RoomDtoId1");
                });

            modelBuilder.Entity("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.PlaylistDto", b =>
                {
                    b.Navigation("PlaylistItems");
                });

            modelBuilder.Entity("Mediaverse.Infrastructure.JointContentConsumption.Repositories.Dtos.RoomDto", b =>
                {
                    b.Navigation("Viewers");
                });
#pragma warning restore 612, 618
        }
    }
}
