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

    public virtual DbSet<InvConstant> InvConstants { get; set; }

    public virtual DbSet<InvCustomer> InvCustomers { get; set; }

    public virtual DbSet<InvCustomersAudit> InvCustomersAudits { get; set; }

    public virtual DbSet<InvInvoice> InvInvoices { get; set; }

    public virtual DbSet<InvInvoiceAudit> InvInvoiceAudits { get; set; }

    public virtual DbSet<InvInvoiceItem> InvInvoiceItems { get; set; }

    public virtual DbSet<InvInvoiceItemsAudit> InvInvoiceItemsAudits { get; set; }

    public virtual DbSet<InvProduct> InvProducts { get; set; }

    public virtual DbSet<InvProductsAudit> InvProductsAudits { get; set; }

    public virtual DbSet<InvProductsStock> InvProductsStocks { get; set; }

    public virtual DbSet<InvUser> InvUsers { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<VactivityLog> VactivityLogs { get; set; }

    public virtual DbSet<Vconstant> Vconstants { get; set; }

    public virtual DbSet<Vcustomer> Vcustomers { get; set; }

    public virtual DbSet<Vinvoice> Vinvoices { get; set; }

    public virtual DbSet<VinvoiceDetail> VinvoiceDetails { get; set; }

    public virtual DbSet<Vproduct> Vproducts { get; set; }

    public virtual DbSet<Vstock> Vstocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Activity__45F4A79138B1BCC7");

            entity.ToTable("ActivityLog");

            entity.Property(e => e.ActionType).HasMaxLength(20);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.RedirectUrl).HasMaxLength(300);
        });

        modelBuilder.Entity<InvConstant>(entity =>
        {
            entity.HasKey(e => e.ConstantId).HasName("PK__InvConst__66315FDF4553432D");

            entity.ToTable("InvConstant");

            entity.HasIndex(e => new { e.Category, e.EntityId, e.Key, e.IsActive }, "UQ_Constant").IsUnique();

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.EntityId).HasMaxLength(100);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PluralName).HasMaxLength(200);
            entity.Property(e => e.ShName).HasMaxLength(200);
            entity.Property(e => e.ShPluralName).HasMaxLength(200);
        });

        modelBuilder.Entity<InvCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerCode).HasName("PK__InvCusto__06678520C5F2ADF9");

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
            entity.HasKey(e => e.AuditId).HasName("PK__InvCusto__A17F23B8317ECF0C");

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

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.InvCustomersAudits)
                .HasForeignKey(d => d.CustomerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvCustomers_Audit_InvCustomers");
        });

        modelBuilder.Entity<InvInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceCode).HasName("PK__InvInvoi__0D9D7FF264C938D0");

            entity.ToTable("InvInvoice", tb => tb.HasTrigger("trg_InvInvoice_Audit"));

            entity.HasIndex(e => e.InvoiceNo, "UQ__InvInvoi__D796B227454AA318").IsUnique();

            entity.Property(e => e.InvoiceCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Gst)
                .HasMaxLength(10)
                .HasColumnName("GST");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.InvInvoices)
                .HasForeignKey(d => d.CustomerCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvCustomer");
        });

        modelBuilder.Entity<InvInvoiceAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__InvInvoi__A17F23B8710ADD85");

            entity.ToTable("InvInvoice_Audit");

            entity.HasIndex(e => e.InvoiceNo, "UQ__InvInvoi__D796B22701ED11E7").IsUnique();

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Gst)
                .HasMaxLength(10)
                .HasColumnName("GST");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.InvoiceCodeNavigation).WithMany(p => p.InvInvoiceAudits)
                .HasForeignKey(d => d.InvoiceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvInvoice_Audit_InvInvoice");
        });

        modelBuilder.Entity<InvInvoiceItem>(entity =>
        {
            entity.HasKey(e => e.ItemCode).HasName("PK__InvInvoi__3ECC0FEB6C153467");

            entity.ToTable("InvInvoice_Items", tb => tb.HasTrigger("trg_InvInvoice_Item_Audit"));

            entity.HasIndex(e => e.InvoiceNo, "UQ__InvInvoi__D796B227AA7883BA").IsUnique();

            entity.Property(e => e.ItemCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Hsncode)
                .HasMaxLength(10)
                .HasColumnName("HSNCode");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.InvoiceCodeNavigation).WithMany(p => p.InvInvoiceItems)
                .HasForeignKey(d => d.InvoiceCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvInvoice");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.InvInvoiceItems)
                .HasForeignKey(d => d.ProductCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvProducts");
        });

        modelBuilder.Entity<InvInvoiceItemsAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__InvInvoi__A17F23B8B1EBDC2C");

            entity.ToTable("InvInvoice_Items_Audit");

            entity.HasIndex(e => e.InvoiceNo, "UQ__InvInvoi__D796B2270D751F60").IsUnique();

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Hsncode)
                .HasMaxLength(10)
                .HasColumnName("HSNCode");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.ItemCodeNavigation).WithMany(p => p.InvInvoiceItemsAudits)
                .HasForeignKey(d => d.ItemCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvInvoice_Items_Audit_InvInvoice_Items");
        });

        modelBuilder.Entity<InvProduct>(entity =>
        {
            entity.HasKey(e => e.ProductCode).HasName("PK__InvProdu__2F4E024E0EE1A9BF");

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_InvProducts_Audit");
                    tb.HasTrigger("trg_InvProducts_InitialStock");
                });

            entity.Property(e => e.ProductCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentStock).HasDefaultValue(1);
            entity.Property(e => e.Hsncode)
                .HasMaxLength(10)
                .HasColumnName("HSNCode");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductLogoMime).HasMaxLength(30);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<InvProductsAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__InvProdu__A17F23B8E3781B19");

            entity.ToTable("InvProducts_Audit");

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentStock).HasDefaultValue(1);
            entity.Property(e => e.Hsncode)
                .HasMaxLength(10)
                .HasColumnName("HSNCode");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ProductLogoMime).HasMaxLength(30);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.InvProductsAudits)
                .HasForeignKey(d => d.ProductCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvProducts_Audit_InvProducts");
        });

        modelBuilder.Entity<InvProductsStock>(entity =>
        {
            entity.HasKey(e => e.StockTnxId).HasName("PK__InvProdu__6079CE38400E3CBF");

            entity.ToTable("InvProducts_Stock", tb => tb.HasTrigger("trg_InvProducts_Stock_Quantity"));

            entity.Property(e => e.StockTnxId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.InvProductsStocks)
                .HasForeignKey(d => d.ProductCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvProducts_Stock_InvProducts");
        });

        modelBuilder.Entity<InvUser>(entity =>
        {
            entity.HasKey(e => e.UserCode).HasName("PK__InvUser__1DF52D0DDA487660");

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
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC078BF5761C");

            entity.Property(e => e.Level).HasMaxLength(128);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D81692AAAAD8");

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

        modelBuilder.Entity<Vconstant>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VConstant");

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.ConstantId).ValueGeneratedOnAdd();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.EntityId).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PluralName).HasMaxLength(200);
            entity.Property(e => e.ShName).HasMaxLength(200);
            entity.Property(e => e.ShPluralName).HasMaxLength(200);
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

        modelBuilder.Entity<Vinvoice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VInvoices");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.Gst)
                .HasMaxLength(10)
                .HasColumnName("GST");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<VinvoiceDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VInvoiceDetail");

            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.Gst)
                .HasMaxLength(10)
                .HasColumnName("GST");
            entity.Property(e => e.Hsncode)
                .HasMaxLength(10)
                .HasColumnName("HSNCode");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.ProductLogoMime).HasMaxLength(30);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Vproduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VProducts");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Hsncode)
                .HasMaxLength(10)
                .HasColumnName("HSNCode");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.PluralShortName).HasMaxLength(200);
            entity.Property(e => e.PluralUnitName).HasMaxLength(200);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.ShortName).HasMaxLength(200);
            entity.Property(e => e.StockDisplay).HasMaxLength(416);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitName).HasMaxLength(200);
            entity.Property(e => e.UnitNameDetail).HasMaxLength(403);
        });

        modelBuilder.Entity<Vstock>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VStock");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.PluralShortName).HasMaxLength(200);
            entity.Property(e => e.PluralUnitName).HasMaxLength(200);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.ShortName).HasMaxLength(200);
            entity.Property(e => e.StockDisplay).HasMaxLength(416);
            entity.Property(e => e.TransactionType).HasMaxLength(200);
            entity.Property(e => e.UnitName).HasMaxLength(200);
            entity.Property(e => e.UnitNameDetail).HasMaxLength(403);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
