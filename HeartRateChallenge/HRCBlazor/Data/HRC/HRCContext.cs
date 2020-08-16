﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace HRCDB.Data.HRC
{
    public partial class HRCContext : DbContext
    {
        public HRCContext()
        {
        }

        public HRCContext(DbContextOptions<HRCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CompetitorHeartRateZones> CompetitorHeartRateZones { get; set; }
        public virtual DbSet<Leaderboard> Leaderboards { get; set; }
        public virtual DbSet<ScoreConfig> ScoreConfigs { get; set; }
        public virtual DbSet<UploadHistory> UploadHistorys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=aspnet-HRCBlazor-E3A21C57-5CEE-4C4F-A49D-8717651DF745;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompetitorHeartRateZones>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK_HeartRateZones");

                entity.ToTable("tbl_CompetitorHeartRateZones");

                entity.Property(e => e.Username).HasMaxLength(256);

                entity.HasOne(d => d.UsernameNavigation)
                    .WithOne(p => p.CompetitorHeartRateZones)
                    .HasForeignKey<CompetitorHeartRateZones>(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HeartRateZones_ToLeaderboard");
            });

            modelBuilder.Entity<Leaderboard>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK_LeaderBoard");

                entity.ToTable("tbl_Leaderboard");

                entity.Property(e => e.Username).HasMaxLength(256);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ScoreConfig>(entity =>
            {
                entity.HasKey(e => e.HeartRateZone)
                    .HasName("PK_ScoreConfig");

                entity.ToTable("tbl_ScoreConfig");

                entity.Property(e => e.HeartRateZone).ValueGeneratedNever();
            });

            modelBuilder.Entity<UploadHistory>(entity =>
            {
                entity.ToTable("tbl_UploadHistory");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsFixedLength();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}