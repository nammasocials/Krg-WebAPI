Use NSinvoiceBilling;
GO
CREATE TABLE [dbo].[InvConstant] (
	[ConstantId] INT IDENTITY(1,1) PRIMARY KEY,
	[EntityId] nvarchar(100) not null,
	[Category] nvarchar(100) not null,
	[Key] int not null,
	[ConstantValue] nvarchar(200),
	[Name] nvarchar(200) not null,
	[PluralName] nvarchar(200),
	[ShName] nvarchar(200) not null,
	[ShPluralName] nvarchar(200),
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
    [ConstantValue],
	[Name],
	[PluralName],
	[ShName],
	[ShPluralName],
	[Description],
	[isActive],
    [CreatedOn],
    [CreatedBy]
    from InvConstant
GO

Insert into [InvConstant] ([EntityId], [Category], [Key],[ConstantValue], [Name],[PluralName], [ShName],[ShPluralName], [Description], [CreatedBy] )
Values('InvProducts','UnitType',1,'1','Piece','Pieces','pc','pcs','a single unit',(Select Top 1 UserCode from InvUser));
Go
Insert into [InvConstant] ([EntityId], [Category], [Key],[ConstantValue], [Name],[PluralName], [ShName],[ShPluralName], [Description], [CreatedBy] )
Values('InvProducts','UnitType',2,'10','Pack','Packs','pk','pks','10 units of Bags',(Select Top 1 UserCode from InvUser))
Go
Insert into [InvConstant] ([EntityId], [Category], [Key],[ConstantValue], [Name], [ShName], [Description], [CreatedBy] )
Values('InvProducts','StockTxnType',1,'Stock-In','1','In','Production of Stock Inventory',(Select Top 1 UserCode from InvUser))
Go
Insert into [InvConstant] ([EntityId],[Category], [Key],[ConstantValue], [Name], [ShName], [Description], [CreatedBy] )
Values('InvProducts','StockTxnType',2,'Stock-Out','-1','Out','Sales of Stock Inventory',(Select Top 1 UserCode from InvUser))
Go