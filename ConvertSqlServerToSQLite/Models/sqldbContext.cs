using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace ConvertSqlServerToSQLite.Models
{
    public partial class sqldbContext : DbContext
    {
        public sqldbContext()
        {
        }

        public sqldbContext(DbContextOptions<sqldbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Personne> Personnes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Personne>(entity =>
            {
                entity.ToTable("personne");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Country)
                    .HasMaxLength(20)
                    .HasColumnName("country")
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .HasColumnName("name")
                    .IsFixedLength();

                entity.Property(e => e.Old).HasColumnName("old");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
