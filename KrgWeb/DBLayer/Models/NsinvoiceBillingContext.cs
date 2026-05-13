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
<<<<<<< Updated upstream
            entity.HasKey(e => e.ActivityId).HasName("PK__Activity__45F4A7918954FF69");
=======
            entity.HasKey(e => e.ActivityId).HasName("PK__Activity__45F4A791E72C414C");
>>>>>>> Stashed changes

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<InvConstant>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.ConstantId).HasName("PK__InvConst__66315FDFD4C04F69");
=======
            entity.HasKey(e => e.ConstantId).HasName("PK__InvConst__66315FDF6379C72B");
>>>>>>> Stashed changes

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<InvCustomer>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.CustomerCode).HasName("PK__InvCusto__06678520DFE54EF6");
=======
            entity.HasKey(e => e.CustomerCode).HasName("PK__InvCusto__06678520CF0E4239");
>>>>>>> Stashed changes

            entity.ToTable(tb => tb.HasTrigger("trg_InvCustomers_Audit"));

            entity.Property(e => e.CustomerCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<InvCustomersAudit>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.AuditId).HasName("PK__InvCusto__A17F23B84C37D664");
=======
            entity.HasKey(e => e.AuditId).HasName("PK__InvCusto__A17F23B89AF5CC59");
>>>>>>> Stashed changes

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.OperationType).IsFixedLength();

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.InvCustomersAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvCustomers_Audit_InvCustomers");
        });

        modelBuilder.Entity<InvInvoice>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.InvoiceCode).HasName("PK__InvInvoi__0D9D7FF2388466D3");

            entity.ToTable("InvInvoice", tb => tb.HasTrigger("trg_InvInvoice_Audit"));

            entity.HasIndex(e => e.InvoiceNo, "UQ__InvInvoi__D796B227E99D0D9C").IsUnique();

=======
            entity.HasKey(e => e.InvoiceCode).HasName("PK__InvInvoi__0D9D7FF20A4B07D3");

            entity.ToTable("InvInvoice", tb => tb.HasTrigger("trg_InvInvoice_Audit"));

>>>>>>> Stashed changes
            entity.Property(e => e.InvoiceCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.InvInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvCustomer");
        });

        modelBuilder.Entity<InvInvoiceAudit>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.AuditId).HasName("PK__InvInvoi__A17F23B8902DDB56");

            entity.ToTable("InvInvoice_Audit");

            entity.HasIndex(e => e.InvoiceNo, "UQ__InvInvoi__D796B2278437557A").IsUnique();

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EwayBillLogo).HasColumnName("EWayBillLogo");
            entity.Property(e => e.EwayBillLogoMime)
                .HasMaxLength(30)
                .HasColumnName("EWayBillLogoMime");
            entity.Property(e => e.Gst)
                .HasMaxLength(10)
                .HasColumnName("GST");
            entity.Property(e => e.InvoiceNo).HasMaxLength(100);
            entity.Property(e => e.IsEwayBillAvailable).HasColumnName("isEwayBillAvailable");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");
=======
            entity.HasKey(e => e.AuditId).HasName("PK__InvInvoi__A17F23B887531600");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.OperationType).IsFixedLength();
>>>>>>> Stashed changes

            entity.HasOne(d => d.InvoiceCodeNavigation).WithMany(p => p.InvInvoiceAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvInvoice_Audit_InvInvoice");
        });

        modelBuilder.Entity<InvInvoiceItem>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.ItemCode).HasName("PK__InvInvoi__3ECC0FEB48E3AC80");

            entity.ToTable("InvInvoice_Items", tb => tb.HasTrigger("trg_InvInvoice_Item_Audit"));

            entity.HasIndex(e => e.InvoiceNo, "UQ__InvInvoi__D796B22736BC9CA4").IsUnique();

=======
            entity.HasKey(e => e.ItemCode).HasName("PK__InvInvoi__3ECC0FEBE66A0FFB");

            entity.ToTable("InvInvoice_Items", tb => tb.HasTrigger("trg_InvInvoice_Item_Audit"));

>>>>>>> Stashed changes
            entity.Property(e => e.ItemCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.InvoiceCodeNavigation).WithMany(p => p.InvInvoiceItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvInvoice");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.InvInvoiceItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvProducts");
        });

        modelBuilder.Entity<InvInvoiceItemsAudit>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.AuditId).HasName("PK__InvInvoi__A17F23B847E14D5B");

            entity.ToTable("InvInvoice_Items_Audit");

            entity.HasIndex(e => e.InvoiceNo, "UQ__InvInvoi__D796B22757483425").IsUnique();

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.CentralGst).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CentralGstAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Cost).HasColumnType("decimal(20, 4)");
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
            entity.Property(e => e.NetProductAmount).HasColumnType("decimal(20, 4)");
            entity.Property(e => e.OperationType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
=======
            entity.HasKey(e => e.AuditId).HasName("PK__InvInvoi__A17F23B8DAD19C71");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.OperationType).IsFixedLength();
>>>>>>> Stashed changes
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.ItemCodeNavigation).WithMany(p => p.InvInvoiceItemsAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvInvoice_Items_Audit_InvInvoice_Items");
        });

        modelBuilder.Entity<InvProduct>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.ProductCode).HasName("PK__InvProdu__2F4E024EEE07FD82");
=======
            entity.HasKey(e => e.ProductCode).HasName("PK__InvProdu__2F4E024EF9DDFB59");
>>>>>>> Stashed changes

            entity.ToTable(tb =>
                {
                    tb.HasTrigger("trg_InvProducts_Audit");
                    tb.HasTrigger("trg_InvProducts_InitialStock");
                });

            entity.Property(e => e.ProductCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CurrentStock).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<InvProductsAudit>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.AuditId).HasName("PK__InvProdu__A17F23B8797C8C17");
=======
            entity.HasKey(e => e.AuditId).HasName("PK__InvProdu__A17F23B85C9A5EEA");
>>>>>>> Stashed changes

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CurrentStock).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.OperationType).IsFixedLength();

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.InvProductsAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvProducts_Audit_InvProducts");
        });

        modelBuilder.Entity<InvProductsStock>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.StockTnxId).HasName("PK__InvProdu__6079CE380D92BF8F");
=======
            entity.HasKey(e => e.StockTnxId).HasName("PK__InvProdu__6079CE38501EBC9D");
>>>>>>> Stashed changes

            entity.ToTable("InvProducts_Stock", tb => tb.HasTrigger("trg_InvProducts_Stock_Quantity"));

            entity.Property(e => e.StockTnxId).HasDefaultValueSql("(newid())");
<<<<<<< Updated upstream
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
=======
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
>>>>>>> Stashed changes

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.InvProductsStocks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvProducts_Stock_InvProducts");
        });

        modelBuilder.Entity<InvUser>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.UserCode).HasName("PK__InvUser__1DF52D0D197FC5A5");

            entity.ToTable("InvUser");
=======
            entity.HasKey(e => e.UserCode).HasName("PK__InvUser__1DF52D0DCBAE6686");
>>>>>>> Stashed changes

            entity.Property(e => e.UserCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.UserTypeNavigation).WithMany(p => p.InvUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvUser_UserType");
        });

        modelBuilder.Entity<Log>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC076180256C");

            entity.Property(e => e.Level).HasMaxLength(128);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
=======
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC073BBC97FB");
>>>>>>> Stashed changes
        });

        modelBuilder.Entity<UserType>(entity =>
        {
<<<<<<< Updated upstream
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D816582EAE76");
=======
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D81654D9CEE3");
>>>>>>> Stashed changes

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<VactivityLog>(entity =>
        {
            entity.ToView("VActivityLog");

            entity.Property(e => e.ActivityId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Vconstant>(entity =>
        {
            entity.ToView("VConstant");

            entity.Property(e => e.ConstantId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Vcustomer>(entity =>
        {
            entity.ToView("VCustomers");
        });

        modelBuilder.Entity<Vinvoice>(entity =>
        {
            entity.ToView("VInvoices");
        });

        modelBuilder.Entity<VinvoiceDetail>(entity =>
        {
            entity.ToView("VInvoiceDetail");
        });

        modelBuilder.Entity<Vproduct>(entity =>
        {
            entity.ToView("VProducts");
        });

        modelBuilder.Entity<Vstock>(entity =>
        {
            entity.ToView("VStock");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
