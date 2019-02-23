using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HalalEcodes.Data
{
    public partial class EcodeContext : DbContext
    {
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Ecode> Ecode { get; set; }

        public EcodeContext(DbContextOptions<EcodeContext> options)
            : base(options)
        {
        }

        // Unable to generate entity type for table 'android_metadata'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite(@"Data Source=C:\\\\MyDev\\\\Repositories\\\\ecodes.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code");

                entity.Property(e => e.Desc)
                    .IsRequired()
                    .HasColumnName("desc");
            });

            modelBuilder.Entity<Ecode>(entity =>
            {
                entity.ToTable("ecode");

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code");

                entity.Property(e => e.ContainsAlcohol).HasColumnName("contains_alcohol");

                entity.Property(e => e.EuApprouved).HasColumnName("eu_approuved");

                entity.Property(e => e.Ingredients).HasColumnName("ingredients");

                entity.Property(e => e.IsToxic).HasColumnName("is_toxic");

                entity.Property(e => e.MainIngredient).HasColumnName("main_ingredient");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.StatusDesc).HasColumnName("status_desc");

                entity.Property(e => e.UsApprouved).HasColumnName("us_approuved");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Ecode)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
