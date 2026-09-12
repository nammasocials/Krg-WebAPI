CREATE TABLE [dbo].[InvInvoice_Audit] (
    [AuditID]              INT              IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [InvoiceCode]          UNIQUEIDENTIFIER NOT NULL,
    [InvoiceNo]            NVARCHAR (100)   NOT NULL,
    [CustomerCode]         UNIQUEIDENTIFIER NOT NULL,
    [isEwayBillAvailable]  BIT              NOT NULL DEFAULT 0,
    [EWayBillLogo]         VARBINARY (MAX)  NULL,
    [EWayBillLogoMime]     NVARCHAR (30)    NULL,
    [TotalCost]            DECIMAL (10, 2)  NOT NULL,
    [GST]                  NVARCHAR (30)    NOT NULL,
    [CreatedOn]            DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [ModifiedOn]           DATETIME         NOT NULL DEFAULT (GETDATE()),
    [ModifiedBy]           UNIQUEIDENTIFIER NULL,
    -- 'I' for Insert, 'U' for Update, 'D' for Delete
    [OperationType]        CHAR (1)         NULL,
    CONSTRAINT [FK_InvInvoice_Audit_InvInvoice] FOREIGN KEY ([InvoiceCode]) REFERENCES [dbo].[InvInvoice] ([InvoiceCode])
);
