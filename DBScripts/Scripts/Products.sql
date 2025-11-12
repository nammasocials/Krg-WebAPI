Use NSinvoiceBilling;
GO
CREATE TABLE [dbo].[InvProducts] (
    [ProductCode] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [ProductName] NVARCHAR(100) NOT NULL,
    [StockCount] Integer Default 0 NOT NULL,
	[UnitName] NVARCHAR(15) NOT NULL,
    [UnitCost] decimal(10,2) NOT Null,
	[ProductLogo] [varbinary](max) NULL,
    [isActive] bit default 1 NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [ModifiedBy] UNIQUEIDENTIFIER
);
Go

CREATE TABLE [dbo].[InvProducts_Audit] (
    [AuditID] INT IDENTITY(1,1) PRIMARY KEY,
    [ProductCode] UNIQUEIDENTIFIER NOT NULL ,
    [ProductName] NVARCHAR(100) NOT NULL,
    [StockCount] Integer Default 0 NOT NULL,
	[UnitName] NVARCHAR(15) NOT NULL,
    [UnitCost] decimal(10,2) NOT Null,
	[ProductLogo] [varbinary](max) NULL,
    [isActive] bit default 1 NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER,
	[ModifiedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [ModifiedBy] UNIQUEIDENTIFIER,
    CONSTRAINT FK_InvProducts_Audit_InvProducts 
    FOREIGN KEY (ProductCode) REFERENCES InvProducts(ProductCode)
);
Go

/****** Object:  View [dbo].[VCustomers]    Script Date: 24/10/2025 14:00:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VProducts] AS
    Select  
	[ProductCode],
    [ProductName],
    [StockCount],
	[UnitName],
    [UnitCost] ,
    [isActive] ,
    [CreatedOn] ,
    [CreatedBy] ,
	[ModifiedOn] ,
    [ModifiedBy] 
    from InvProducts;
GO