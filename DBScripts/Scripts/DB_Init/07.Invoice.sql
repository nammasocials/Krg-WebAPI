Use NSinvoiceBilling;
GO
CREATE TABLE [dbo].[InvInvoice] (
    [InvoiceCode] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [InvoiceNo] NVARCHAR(100) NOT NULL unique,
	[CustomerCode] UNIQUEIDENTIFIER NOT NULL,
	[isEwayBillAvailable] bit DEFAULT 0 Not null,
	[EWayBillLogo] [varbinary](max) NULL,
    [TotalCost] decimal(10,2) NOT Null,
	[GST] nvarchar(10) not null,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [ModifiedBy] UNIQUEIDENTIFIER,
	CONSTRAINT FK_InvCustomer
    FOREIGN KEY (CustomerCode) REFERENCES InvCustomers(CustomerCode)
);
Go

CREATE TABLE [dbo].[InvInvoice_Items] (
    [ItemCode] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
	[InvoiceCode] UNIQUEIDENTIFIER NOT NULL,
    [InvoiceNo] NVARCHAR(100) NOT NULL unique,
	[ProductCode] UNIQUEIDENTIFIER NOT NULL,
    [UnitCost] decimal(10,2) NOT Null,
	[Cost] decimal(10,2) NOT Null,
	[HSNCode] nvarchar(10) not null,
	[Quantity] int DEFAULT 1 NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [ModifiedBy] UNIQUEIDENTIFIER,
	CONSTRAINT FK_InvInvoice
    FOREIGN KEY (InvoiceCode) REFERENCES InvInvoice(InvoiceCode),
	CONSTRAINT FK_InvProducts
    FOREIGN KEY (ProductCode) REFERENCES InvProducts(ProductCode)
);
Go
CREATE TABLE [dbo].[InvInvoice_Audit] (
    [AuditID] INT IDENTITY(1,1) PRIMARY KEY,
    [InvoiceCode] UNIQUEIDENTIFIER NOT NULL,
    [InvoiceNo] NVARCHAR(100) NOT NULL unique,
	[CustomerCode] UNIQUEIDENTIFIER NOT NULL,
	[isEwayBillAvailable] bit DEFAULT 0 Not null,
	[EWayBillLogo] [varbinary](max) NULL,
    [TotalCost] decimal(10,2) NOT Null,
	[GST] nvarchar(10) not null,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [ModifiedBy] UNIQUEIDENTIFIER,
    -- ... more columns ...
    OperationType CHAR(1), -- 'I' for Insert, 'U' for Update, 'D' for Delete
    CONSTRAINT FK_InvInvoice_Audit_InvInvoice 
    FOREIGN KEY (InvoiceCode) REFERENCES InvInvoice(InvoiceCode)
);
Go

CREATE TABLE [dbo].[InvInvoice_Items_Audit] (
    [AuditID] INT IDENTITY(1,1) PRIMARY KEY,
    [ItemCode] UNIQUEIDENTIFIER NOT NULL,
	[InvoiceCode] UNIQUEIDENTIFIER NOT NULL,
    [InvoiceNo] NVARCHAR(100) NOT NULL unique,
	[ProductCode] UNIQUEIDENTIFIER NOT NULL,
    [UnitCost] decimal(10,2) NOT Null,
	[Cost] decimal(10,2) NOT Null,
	[HSNCode] nvarchar(10) not null,
	[Quantity] int DEFAULT 1 NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [ModifiedBy] UNIQUEIDENTIFIER,
    -- ... more columns ...
    OperationType CHAR(1), -- 'I' for Insert, 'U' for Update, 'D' for Delete
    CONSTRAINT FK_InvInvoice_Items_Audit_InvInvoice_Items
    FOREIGN KEY (ItemCode) REFERENCES InvInvoice_Items(ItemCode)
);
Go

CREATE OR ALTER VIEW [dbo].[VInvoices] AS
SELECT
      Inv.InvoiceCode,
      Inv.InvoiceNo,
	  C.CustomerName,
	  Inv.isEwayBillAvailable,
      Inv.TotalCost,
      Inv.GST,
      Inv.[CreatedOn],
      Inv.[CreatedBy],
      Inv.[ModifiedOn],
      Inv.[ModifiedBy]
	  FROM InvInvoice Inv
	  INNER JOIN InvCustomers C
       ON Inv.CustomerCode = C.CustomerCode
GO

CREATE OR ALTER VIEW [dbo].[VInvoiceDetail] AS
SELECT
      Inv.InvoiceCode,
	  InvItems.ItemCode,
      Inv.InvoiceNo,
	  C.CustomerName,
	  InvItems.ProductCode,
	  P.ProductName,
	  P.ProductLogo,
	  P.ProductLogoMime,
	  InvItems.UnitCost,
	  InvItems.Cost,
	  InvItems.Quantity,
	  InvItems.HSNCode,
      Inv.TotalCost,
      Inv.GST,
      InvItems.[CreatedOn],
      InvItems.[CreatedBy],
      InvItems.[ModifiedOn],
      InvItems.[ModifiedBy]
	  FROM InvInvoice_Items InvItems
	  INNER JOIN InvProducts P
       ON InvItems.ProductCode = P.ProductCode
	  INNER JOIN InvInvoice Inv
       ON Inv.InvoiceCode = InvItems.InvoiceCode 
	  INNER JOIN InvCustomers C
       ON Inv.CustomerCode = C.CustomerCode
GO