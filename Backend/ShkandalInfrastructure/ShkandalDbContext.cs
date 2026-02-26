using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShkandalData.Models;
using System.Reflection.Emit;
namespace ShkandalInfrastructure
{
    public class ShkandalDbContext : DbContext
    {
        public ShkandalDbContext(DbContextOptions<ShkandalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pg_trgm");

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("article");

                entity.HasKey(a => a.Id);
                entity.Property(a => a.MediaId).HasColumnName("media_id");
                entity.Property(a => a.ClusterId).HasColumnName("cluster_id");
                entity.Property(a => a.FeaturedImageURL).HasColumnName("featured_image_url");
                entity.Property(a => a.IsRelevant).HasColumnName("is_relevant");
                entity.Property(a => a.PublishedAt).HasColumnName("published_at");
                entity.Property(a => a.IsChecked)
                      .HasColumnName("is_checked")
                      .HasDefaultValue(false);

                entity.HasOne(a => a.Media)
                      .WithMany(m => m.Articles)
                      .HasForeignKey(a => a.MediaId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Cluster)
                      .WithMany(c => c.Articles)
                      .HasForeignKey(a => a.ClusterId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(a => a.Event)
                    .WithMany(e => e.Articles)
                    .HasForeignKey(a => a.EventId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("app_user");

                entity.HasKey(u => u.Id);
                entity.Property(u => u.PasswordHash).HasColumnName("password_hash");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.HasKey(c => c.Id);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event");

                entity.HasKey(e => e.Id);
                entity.Property(c => c.EventTime).HasColumnName("event_time");

                entity.HasOne(e => e.Cluster)
                     .WithMany(c => c.Events)
                     .HasForeignKey(e => e.ClusterId)
                     .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Cluster>(entity =>
            {
                entity.ToTable("cluster");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.IsRelevant).HasColumnName("is_relevant");
                entity.Property(c => c.ViewCounter).HasColumnName("view_counter");
                entity.Property(c => c.FeaturedImageURL).HasColumnName("featured_image_url");
                entity.Property(c => c.LastUpdatedAt).HasColumnName("last_updated_at");

                entity.HasMany(c => c.Categories)
                   .WithMany(ca => ca.Clusters);
            });


            modelBuilder.Entity<Media>(entity =>
            {
                entity.ToTable("media");

                entity.HasKey(m => m.Id);
                entity.Property(m => m.IsActive).HasColumnName("is_active");
                entity.Property(m => m.SitemapIndexURL).HasColumnName("sitemap_index_url");
            });

        }
    }
}
