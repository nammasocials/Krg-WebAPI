Use NSinvoiceBilling;
GO
CREATE TABLE [dbo].[InvConstant] (
	[ConstantId] INT IDENTITY(1,1) PRIMARY KEY,
	[EntityId] nvarchar(100) not null,
	[Category] nvarchar(100) not null,
	[Key] int not null,
	[Name] nvarchar(200) not null,
	[ShName] nvarchar(200) not null,
	[Description] nvarchar(200),
	[isActive] bit default 1 NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER,
	CONSTRAINT UQ_Constant UNIQUE ([Category],[EntityId], [Key], [isActive])
);
GO

CREATE OR ALTER View VConstant AS
	Select  
	[ConstantId],
	[EntityId],
	[Category],
	[Key],
	[Name],
	[ShName],
	[Description],
	[isActive],
    [CreatedOn],
    [CreatedBy]
    from InvConstant
GO

Insert into [InvConstant] ([EntityId], [Category], [Key], [Name], [ShName], [Description], [CreatedBy] )
Values('InvProducts','UnitType',1,'Piece','pcs','a single unit',(Select Top 1 UserCode from InvUser));
Go
Insert into [InvConstant] ([EntityId], [Category], [Key], [Name], [ShName], [Description], [CreatedBy] )
Values('InvProducts','UnitType',2,'Pack','pk','10 units of Bags',(Select Top 1 UserCode from InvUser))
Go
Insert into [InvConstant] ([EntityId], [Category], [Key], [Name], [ShName], [Description], [CreatedBy] )
Values('InvProducts','StockTxnType',1,'Stock-In','In','Production of Stock Inventory',(Select Top 1 UserCode from InvUser))
Go
Insert into [InvConstant] ([EntityId],[Category], [Key], [Name], [ShName], [Description], [CreatedBy] )
Values('InvProducts','StockTxnType',2,'Stock-Out','Out','Sales of Stock Inventory',(Select Top 1 UserCode from InvUser))
Go