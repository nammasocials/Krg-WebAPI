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

    public virtual DbSet<InvExpense> InvExpenses { get; set; }

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

    public virtual DbSet<Vexpense> Vexpenses { get; set; }

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
            entity.HasKey(e => e.ActivityId).HasName("PK__Activity__45F4A7913624A68B");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<InvConstant>(entity =>
        {
            entity.HasKey(e => e.ConstantId).HasName("PK__InvConst__66315FDF524730C3");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<InvCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerCode).HasName("PK__InvCusto__06678520F85CD989");

            entity.ToTable(tb => tb.HasTrigger("trg_InvCustomers_Audit"));

            entity.Property(e => e.CustomerCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<InvCustomersAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__InvCusto__A17F23B84B857908");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.OperationType).IsFixedLength();

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.InvCustomersAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvCustomers_Audit_InvCustomers");
        });

        modelBuilder.Entity<InvInvoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceCode).HasName("PK__InvInvoi__0D9D7FF2A5D0DA89");

            entity.ToTable("InvInvoice", tb => tb.HasTrigger("trg_InvInvoice_Audit"));

            entity.Property(e => e.InvoiceCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.CustomerCodeNavigation).WithMany(p => p.InvInvoices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvCustomer");
        });

        modelBuilder.Entity<InvInvoiceAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__InvInvoi__A17F23B8C665B51F");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.OperationType).IsFixedLength();

            entity.HasOne(d => d.InvoiceCodeNavigation).WithMany(p => p.InvInvoiceAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvInvoice_Audit_InvInvoice");
        });

        modelBuilder.Entity<InvInvoiceItem>(entity =>
        {
            entity.HasKey(e => e.ItemCode).HasName("PK__InvInvoi__3ECC0FEBA30AB1E9");

            entity.ToTable("InvInvoice_Items", tb =>
                {
                    tb.HasTrigger("trg_AfterInsert_InvInvoiceItems");
                    tb.HasTrigger("trg_InvInvoice_Item_Audit");
                });

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
            entity.HasKey(e => e.AuditId).HasName("PK__InvInvoi__A17F23B8784D4C29");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.OperationType).IsFixedLength();
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.ItemCodeNavigation).WithMany(p => p.InvInvoiceItemsAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvInvoice_Items_Audit_InvInvoice_Items");
        });

        modelBuilder.Entity<InvProduct>(entity =>
        {
            entity.HasKey(e => e.ProductCode).HasName("PK__InvProdu__2F4E024E7253D5EA");

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
            entity.HasKey(e => e.AuditId).HasName("PK__InvProdu__A17F23B8BB0ADCB8");

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
            entity.HasKey(e => e.StockTnxId).HasName("PK__InvProdu__6079CE3845F29430");

            entity.ToTable("InvProducts_Stock", tb => tb.HasTrigger("trg_InvProducts_Stock_Quantity"));

            entity.Property(e => e.StockTnxId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ProductCodeNavigation).WithMany(p => p.InvProductsStocks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvProducts_Stock_InvProducts");
        });

        modelBuilder.Entity<InvUser>(entity =>
        {
            entity.HasKey(e => e.UserCode).HasName("PK__InvUser__1DF52D0D89E0D5F2");

            entity.Property(e => e.UserCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.UserTypeNavigation).WithMany(p => p.InvUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvUser_UserType");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC0798F485DE");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D81694E3940E");

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

        modelBuilder.Entity<InvExpense>(entity =>
        {
            entity.HasKey(e => e.ExpenseCode);

            entity.ToTable(tb => tb.HasTrigger("trg_InvExpenses_Audit"));

            entity.Property(e => e.ExpenseCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ExpenseDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedOn).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Vexpense>(entity =>
        {
            entity.ToView("VExpenses");
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
