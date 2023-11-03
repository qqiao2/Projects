using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ImageMeasurement.DataModels;

public partial class TestDbContext : DbContext
{
    public TestDbContext()
    {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AnatomicalFeature> AnatomicalFeatures { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Measurement> Measurements { get; set; }

    public virtual DbSet<MeasurementAuditTrail> MeasurementAuditTrails { get; set; }

    public virtual DbSet<MeasurementType> MeasurementTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=WonderLand;Database=TestDB;User Id=sa;Password=WuFamily6; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnatomicalFeature>(entity =>
        {
            entity.ToTable("AnatomicalFeature");

            entity.HasIndex(e => e.Name, "UQ__Anatomic__737584F6E5502240").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false);

            entity.HasMany(d => d.MeasurementTypes).WithMany(p => p.AnatomicalFeatures)
                .UsingEntity<Dictionary<string, object>>(
                    "MeasurementTypeAnatomicalFeature",
                    r => r.HasOne<MeasurementType>().WithMany()
                        .HasForeignKey("MeasurementTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Measureme__Measu__300424B4"),
                    l => l.HasOne<AnatomicalFeature>().WithMany()
                        .HasForeignKey("AnatomicalFeatureId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Measureme__Anato__2F10007B"),
                    j =>
                    {
                        j.HasKey("AnatomicalFeatureId", "MeasurementTypeId");
                        j.ToTable("MeasurementTypeAnatomicalFeature");
                        j.IndexerProperty<int>("AnatomicalFeatureId").HasColumnName("AnatomicalFeatureID");
                        j.IndexerProperty<int>("MeasurementTypeId").HasColumnName("MeasurementTypeID");
                    });
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");

            entity.HasIndex(e => e.InstanceUid, "UQ__Image__691195789E8053DB").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FileLocation)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.InstanceUid)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("InstanceUID");
            entity.Property(e => e.Modality)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.SeriesInstanceUid)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("SeriesInstanceUID");
        });

        modelBuilder.Entity<Measurement>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AnatomicalFeatureId).HasColumnName("AnatomicalFeatureID");
            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.MeasurementText).IsUnicode(false);
            entity.Property(e => e.MeasurementTypeId).HasColumnName("MeasurementTypeID");

            entity.HasOne(d => d.AnatomicalFeature).WithMany(p => p.Measurements)
                .HasForeignKey(d => d.AnatomicalFeatureId)
                .HasConstraintName("FK__Measureme__Anato__33D4B598");

            entity.HasOne(d => d.Image).WithMany(p => p.Measurements)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("FK__Measureme__Image__32E0915F");

            entity.HasOne(d => d.MeasurementType).WithMany(p => p.Measurements)
                .HasForeignKey(d => d.MeasurementTypeId)
                .HasConstraintName("FK__Measureme__Measu__34C8D9D1");
        });

        modelBuilder.Entity<MeasurementAuditTrail>(entity =>
        {
            entity.ToTable("MeasurementAuditTrail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Action)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Intent)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.MeasurementId).HasColumnName("MeasurementID");
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Measurement).WithMany(p => p.MeasurementAuditTrails)
                .HasForeignKey(d => d.MeasurementId)
                .HasConstraintName("FK__Measureme__Measu__37A5467C");

            entity.HasOne(d => d.User).WithMany(p => p.MeasurementAuditTrails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Measureme__UserI__38996AB5");
        });

        modelBuilder.Entity<MeasurementType>(entity =>
        {
            entity.ToTable("MeasurementType");

            entity.HasIndex(e => e.Name, "UQ__Measurem__737584F6083AB2FD").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.LoginPassWord)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
