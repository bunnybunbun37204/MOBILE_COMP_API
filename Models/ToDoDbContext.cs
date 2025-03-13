using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ToDo.Models
{
    public partial class ToDoDbContext : DbContext
    {
        public ToDoDbContext()
        {
        }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activity { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3309;user=myuser;password=mypassword;database=mydatabase", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.6.2-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_uca1400_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasIndex(e => e.UserId, "FK_Activity_UserId");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UserId).HasMaxLength(13);

                entity.Property(e => e.When).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Activity_UserId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(13)
                    .HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(44)
                    .HasColumnName("password");

                entity.Property(e => e.Salt)
                    .HasMaxLength(24)
                    .HasColumnName("salt");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
