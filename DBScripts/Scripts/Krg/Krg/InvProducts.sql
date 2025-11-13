--CREATE DATABASE NSinvoiceBilling;
--GO
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
    -- ... more columns ...
    OperationType CHAR(1), -- 'I' for Insert, 'U' for Update, 'D' for Delete
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

CREATE OR ALTER TRIGGER trg_InvProducts_Audit
ON InvProducts
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO InvProducts_Audit (
        [ProductCode]
        ,[ProductName]
        ,[StockCount]
        ,[UnitName]
        ,[UnitCost]
        ,[ProductLogo]
        ,[isActive]
        ,[CreatedOn]
        ,[CreatedBy]
        ,[ModifiedOn]
        ,[ModifiedBy]
        ,[OperationType]
    )
    SELECT
        i.[ProductCode]
        ,i.[ProductName]
        ,i.[StockCount]
        ,i.[UnitName]
        ,i.[UnitCost]
        ,i.[ProductLogo]
        ,i.[isActive]
        ,i.[CreatedOn]
        ,i.[CreatedBy]
        ,i.[ModifiedOn]
        ,i.[ModifiedBy]
        ,'I'
    FROM inserted i
    LEFT JOIN deleted d ON i.[ProductCode] = d.[ProductCode]
    WHERE d.[ProductCode] IS NULL; -- Only rows newly inserted

    -- Insert audit for deleted rows (DELETE)
    INSERT INTO InvProducts_Audit (
        [ProductCode]
        ,[ProductName]
        ,[StockCount]
        ,[UnitName]
        ,[UnitCost]
        ,[ProductLogo]
        ,[isActive]
        ,[CreatedOn]
        ,[CreatedBy]
        ,[ModifiedOn]
        ,[ModifiedBy]
        ,[OperationType]
    )
    SELECT
        d.[ProductCode]
        ,d.[ProductName]
        ,d.[StockCount]
        ,d.[UnitName]
        ,d.[UnitCost]
        ,d.[ProductLogo]
        ,d.[isActive]
        ,d.[CreatedOn]
        ,d.[CreatedBy]
        ,d.[ModifiedOn]
        ,d.[ModifiedBy]
        ,'D'
    FROM deleted d
    LEFT JOIN inserted i ON i.[ProductCode] = d.[ProductCode]
    WHERE i.[ProductCode] IS NULL; -- Only rows deleted

    -- Insert audit for updated rows (UPDATE)
    INSERT INTO InvProducts_Audit (
        [ProductCode]
        ,[ProductName]
        ,[StockCount]
        ,[UnitName]
        ,[UnitCost]
        ,[ProductLogo]
        ,[isActive]
        ,[CreatedOn]
        ,[CreatedBy]
        ,[ModifiedOn]
        ,[ModifiedBy]
        ,[OperationType]
    )
    SELECT
        i.[ProductCode]
        ,i.[ProductName]
        ,i.[StockCount]
        ,i.[UnitName]
        ,i.[UnitCost]
        ,i.[ProductLogo]
        ,i.[isActive]
        ,i.[CreatedOn]
        ,i.[CreatedBy]
        ,i.[ModifiedOn]
        ,i.[ModifiedBy]
        ,'U'
    FROM inserted i
    JOIN deleted d ON i.[ProductCode] = d.[ProductCode];
END