--CREATE DATABASE NSinvoiceBilling;
--GO
Use NSinvoiceBilling;
GO
CREATE TABLE [dbo].[InvProducts_Constant] (
	[ConstantId] INT IDENTITY(1,1) PRIMARY KEY,
	[Category] nvarchar(100),
	[Key] int not null,
	[Name] nvarchar(200) not null,
	[ShName] nvarchar(200) not null,
	[Description] nvarchar(200),
	[isActive] bit default 1 NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER,
	CONSTRAINT UQ_InvProducts_Constant UNIQUE ([Category], [Key], [isActive])
);
GO

Insert into [InvProducts_Constant] ([Category], [Key], [Name], [ShName], [Description], [CreatedBy] )
Values('UnitType',1,'Piece','pcs','a single unit',(Select Top 1 UserCode from InvUser));
Go
Insert into [InvProducts_Constant] ([Category], [Key], [Name], [ShName], [Description], [CreatedBy] )
Values('UnitType',2,'Pack','pk','10 units of Bags',(Select Top 1 UserCode from InvUser))
Go
Insert into [InvProducts_Constant] ([Category], [Key], [Name], [ShName], [Description], [CreatedBy] )
Values('StockTxnType',1,'Stock-In','In','Production of Stock Inventory',(Select Top 1 UserCode from InvUser))
Go
Insert into [InvProducts_Constant] ([Category], [Key], [Name], [ShName], [Description], [CreatedBy] )
Values('StockTxnType',2,'Stock-Out','Out','Sales of Stock Inventory',(Select Top 1 UserCode from InvUser))
Go

CREATE TABLE [dbo].[InvProducts] (
    [ProductCode] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [ProductName] NVARCHAR(100) NOT NULL,
	[UnitType] int NOT NULL,
    [UnitCost] decimal(10,2) NOT Null,
	[CurrentStock] int DEFAULT 1 NOT NULL,
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
	[UnitType] int NOT NULL,
    [UnitCost] decimal(10,2) NOT Null,
	[CurrentStock] int DEFAULT 1 NOT NULL,
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
    [CurrentStock],
	unit.[Name] as UnitName,
    [UnitCost] ,
    P.[isActive] ,
    P.[CreatedOn] ,
    P.[CreatedBy] ,
	[ModifiedOn] ,
    [ModifiedBy] 
    from InvProducts P 
	inner join InvProducts_Constant unit on unit.[Key] = P.UnitType and Category = 'UnitType';
GO

CREATE OR ALTER TRIGGER trg_InvProducts_InitialStock
ON InvProducts
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
	INSERT INTO [dbo].[InvProducts_Stock]
           ([ProductCode]
           ,[UnitType]
           ,[Quantity]
           ,[TxnType]
           ,[CreatedBy])
     SELECT
        i.[ProductCode]
        ,i.[UnitType]
		,i.[CurrentStock]
		,(Select [key] from InvProducts_Constant where  Category = 'StockTxnType' and Name = 'Stock-In')
        ,i.[CreatedBy]
    FROM inserted i
END
GO

CREATE OR ALTER TRIGGER trg_InvProducts_Audit
ON InvProducts
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO InvProducts_Audit (
		[ProductCode],
		[ProductName],
		[UnitType],
		[UnitCost],
		[CurrentStock],
		[ProductLogo],
		[isActive],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		[OperationType]
    )
    SELECT
        i.[ProductCode]
        ,i.[ProductName]
        ,i.[UnitType]
        ,i.[UnitCost]
		,i.[CurrentStock]
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
        [ProductCode],
		[ProductName],
		[UnitType],
		[UnitCost],
		[CurrentStock],
		[ProductLogo],
		[isActive],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		[OperationType]
    )
    SELECT
        d.[ProductCode]
        ,d.[ProductName]
        ,d.[UnitType]
        ,d.[UnitCost]
		,d.[CurrentStock]
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
        [ProductCode],
		[ProductName],
		[UnitType],
		[UnitCost],
		[CurrentStock],
		[ProductLogo],
		[isActive],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		[OperationType]
    )
    SELECT
        i.[ProductCode]
        ,i.[ProductName]
        ,i.[UnitType]
        ,i.[UnitCost]
		,i.[CurrentStock]
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
GO