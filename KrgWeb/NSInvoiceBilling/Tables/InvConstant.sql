CREATE TABLE [dbo].[InvConstant] (
    [ConstantId]    INT              IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [EntityId]      NVARCHAR (100)   NOT NULL,
    [Category]      NVARCHAR (100)   NOT NULL,
    [Key]           INT              NOT NULL,
    [ConstantValue] NVARCHAR (200)   NULL,
    [Name]          NVARCHAR (200)   NOT NULL,
    [PluralName]    NVARCHAR (200)   NULL,
    [ShName]        NVARCHAR (200)   NOT NULL,
    [ShPluralName]  NVARCHAR (200)   NULL,
    [Description]   NVARCHAR (200)   NULL,
    [isActive]      BIT              NOT NULL DEFAULT 1,
    [CreatedOn]     DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]     UNIQUEIDENTIFIER NULL,
    CONSTRAINT [UQ_Constant] UNIQUE ([Category], [EntityId], [Key], [isActive])
);
