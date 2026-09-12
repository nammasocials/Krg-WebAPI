CREATE TABLE [dbo].[InvProducts_Audit] (
    [AuditID]         INT              IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [ProductCode]     UNIQUEIDENTIFIER NOT NULL,
    [ProductName]     NVARCHAR (100)   NOT NULL,
    [UnitCost]        DECIMAL (10, 2)  NOT NULL,
    [HSNCode]         NVARCHAR (10)    NOT NULL,
    [CentralGstPer]   DECIMAL (10, 2)  NOT NULL,
    [StateGstPer]     DECIMAL (10, 2)  NOT NULL,
    [CurrentStock]    INT              NOT NULL DEFAULT 1,
    [ProductLogo]     VARBINARY (MAX)  NULL,
    [ProductLogoMime] NVARCHAR (30)    NULL,
    [isActive]        BIT              NOT NULL DEFAULT 1,
    [CreatedOn]       DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [ModifiedOn]      DATETIME         NOT NULL DEFAULT (GETDATE()),
    [ModifiedBy]      UNIQUEIDENTIFIER NULL,
    -- 'I' for Insert, 'U' for Update, 'D' for Delete
    [OperationType]   CHAR (1)         NULL,
    CONSTRAINT [FK_InvProducts_Audit_InvProducts] FOREIGN KEY ([ProductCode]) REFERENCES [dbo].[InvProducts] ([ProductCode])
);
