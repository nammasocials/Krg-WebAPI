using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBLayer.Models;

public partial class NsinvoiceBillingContext : DbContext
{
    public NsinvoiceBillingContext()
    {
    }

    public NsinvoiceBillingContext(DbContextOptions<NsinvoiceBillingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<InvCustomer> InvCustomers { get; set; }

    public virtual DbSet<InvCustomersAudit> InvCustomersAudits { get; set; }

    public virtual DbSet<InvProduct> InvProducts { get; set; }

    public virtual DbSet<InvUser> InvUsers { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<VactivityLog> VactivityLogs { get; set; }

    public virtual DbSet<Vcustomer> Vcustomers { get; set; }

    public virtual DbSet<Vproduct> Vproducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Activity__45F4A7917FB70C2E");

            entity.ToTable("ActivityLog");

            entity.Property(e => e.ActionType).HasMaxLength(20);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.RedirectUrl).HasMaxLength(300);
        });

        modelBuilder.Entity<InvCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerCode).HasName("PK__InvCusto__06678520CBE5978E");

            entity.ToTable(tb => tb.HasTrigger("trg_InvCustomers_Audit"));

            entity.Property(e => e.CustomerCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ContactNo).HasMaxLength(15);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerAddress).HasMaxLength(500);
            entity.Property(e => e.CustomerEmail).HasMaxLength(100);
            entity.Property(e => e.CustomerLogoMime).HasMaxLength(30);
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.Gst)
                .HasMaxLength(30)
                .HasColumnName("GST");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.SecnContactNo).HasMaxLength(15);
        });

        modelBuilder.Entity<InvCustomersAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__InvCusto__A17F23B8B20FC2AB");

            entity.ToTable("InvCustomers_Audit");

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.ContactNo).HasMaxLength(15);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerAddress).HasMaxLength(500);
            entity.Property(e => e.CustomerEmail).HasMaxLength(100);
            entity.Property(e => e.CustomerLogoMime).HasMaxLength(30);
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.Gst)
                .HasMaxLength(30)
                .HasColumnName("GST");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.OperationType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SecnContactNo).HasMaxLength(15);
        });

        modelBuilder.Entity<InvProduct>(entity =>
        {
            entity.HasKey(e => e.ProductCode).HasName("PK__InvProdu__2F4E024EED2F3AA0");

            entity.Property(e => e.ProductCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitName).HasMaxLength(15);
        });

        modelBuilder.Entity<InvUser>(entity =>
        {
            entity.HasKey(e => e.UserCode).HasName("PK__InvUser__1DF52D0DABA43E35");

            entity.ToTable("InvUser");

            entity.Property(e => e.UserCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.UserTypeNavigation).WithMany(p => p.InvUsers)
                .HasForeignKey(d => d.UserType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvUser_UserType");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC07DD2A561E");

            entity.Property(e => e.Level).HasMaxLength(128);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D8168F7E2F0F");

            entity.ToTable("UserType");

            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.UserTypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<VactivityLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VActivityLog");

            entity.Property(e => e.ActionType).HasMaxLength(20);
            entity.Property(e => e.ActivityId).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.RedirectUrl).HasMaxLength(300);
        });

        modelBuilder.Entity<Vcustomer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VCustomers");

            entity.Property(e => e.ContactNo).HasMaxLength(15);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CustomerAddress).HasMaxLength(500);
            entity.Property(e => e.CustomerEmail).HasMaxLength(100);
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.Gst)
                .HasMaxLength(30)
                .HasColumnName("GST");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.SecnContactNo).HasMaxLength(15);
        });

        modelBuilder.Entity<Vproduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VProducts");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitName).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
