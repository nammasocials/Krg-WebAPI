Use NSinvoiceBilling;
GO
CREATE TABLE [dbo].[InvInvoice] (
    [InvoiceCode] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [InvoiceNo] NVARCHAR(100) NOT NULL unique,
	[CustomerCode] UNIQUEIDENTIFIER NOT NULL,
	[isEwayBillAvailable] bit DEFAULT 0 Not null,
	[EWayBillLogo] [varbinary](max) NULL,
	[EWayBillLogoMime] Nvarchar(30),
	[GST] Nvarchar(30) NOT Null,
	[TotalCost] decimal(10,2) NOT Null,
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
    [InvoiceNo] NVARCHAR(100) NOT NULL,
	[ProductCode] UNIQUEIDENTIFIER NOT NULL,
    [UnitCost] decimal(10,2) NOT Null,
	[Quantity] int DEFAULT 1 NOT NULL,
	[Cost] decimal(20,4) NOT Null,
	[HSNCode] nvarchar(10) not null,
	[CentralGst] decimal(10,2) NOT Null,
	[CentralGstAmount] decimal(10,2) NOT Null,
	[StateGst] decimal(10,2) NOT Null,
	[StateGstAmount] decimal(10,2) NOT Null,
	[NetProductAmount] decimal(20,4) NOT Null,
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
    [InvoiceNo] NVARCHAR(100) NOT NULL,
	[CustomerCode] UNIQUEIDENTIFIER NOT NULL,
	[isEwayBillAvailable] bit DEFAULT 0 Not null,
	[EWayBillLogo] [varbinary](max) NULL,
	[EWayBillLogoMime] Nvarchar(30),
    [TotalCost] decimal(10,2) NOT Null,
	[GST] Nvarchar(30) NOT Null,
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
    [InvoiceNo] NVARCHAR(100) NOT NULL,
	[ProductCode] UNIQUEIDENTIFIER NOT NULL,
    [UnitCost] decimal(10,2) NOT Null,
	[Quantity] int DEFAULT 1 NOT NULL,
	[Cost] decimal(20,4) NOT Null,
	[HSNCode] nvarchar(10) not null,
	[CentralGst] decimal(10,2) NOT Null,
	[CentralGstAmount] decimal(10,2) NOT Null,
	[StateGst] decimal(10,2) NOT Null,
	[StateGstAmount] decimal(10,2) NOT Null,
	[NetProductAmount] decimal(20,4) NOT Null,
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
	  C.CustomerCode,
	  C.GST as GSTNumber,
	  C.CustomerEmail,
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
	  c.CustomerCode,
	  C.GST as GSTNumber,
	  C.CustomerEmail,
	  InvItems.ProductCode,
	  P.ProductName,
	  P.ProductLogo,
	  P.ProductLogoMime,
	  InvItems.UnitCost,
	  InvItems.Quantity,
	  InvItems.Cost,
	  InvItems.CentralGst,
	  InvItems.CentralGstAmount,
	  InvItems.StateGst,
	  InvItems.StateGstAmount,
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