CREATE TABLE [dbo].[InvCustomers_Audit] (
    [AuditID]           INT              IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [CustomerCode]      UNIQUEIDENTIFIER NOT NULL,
    [CustomerName]      NVARCHAR (100)   NOT NULL,
    [CustomerEmail]     NVARCHAR (100)   NOT NULL,
    [ContactNo]         NVARCHAR (15)    NOT NULL,
    [SecnContactNo]     NVARCHAR (15)    NULL,
    [CustomerAddress]   NVARCHAR (500)   NOT NULL,
    [GST]               NVARCHAR (30)    NOT NULL,
    [CustomerLogo]      VARBINARY (MAX)  NULL,
    [CustomerLogoMime]  NVARCHAR (30)    NULL,
    [CreatedOn]         DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]         UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]        DATETIME         NULL,
    [ModifiedBy]        UNIQUEIDENTIFIER NULL,
    -- 'I' for Insert, 'U' for Update, 'D' for Delete
    [OperationType]     CHAR (1)         NULL,
    CONSTRAINT [FK_InvCustomers_Audit_InvCustomers] FOREIGN KEY ([CustomerCode]) REFERENCES [dbo].[InvCustomers] ([CustomerCode])
);
