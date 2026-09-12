CREATE TABLE [dbo].[InvInvoice_Items_Audit] (
    [AuditID]          INT              IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [ItemCode]         UNIQUEIDENTIFIER NOT NULL,
    [InvoiceCode]      UNIQUEIDENTIFIER NOT NULL,
    [InvoiceNo]        NVARCHAR (100)   NOT NULL,
    [ProductCode]      UNIQUEIDENTIFIER NOT NULL,
    [UnitCost]         DECIMAL (10, 2)  NOT NULL,
    [Quantity]         INT              NOT NULL DEFAULT 1,
    [Cost]             DECIMAL (20, 4)  NOT NULL,
    [HSNCode]          NVARCHAR (10)    NOT NULL,
    [CentralGst]       DECIMAL (10, 2)  NOT NULL,
    [CentralGstAmount] DECIMAL (10, 2)  NOT NULL,
    [StateGst]         DECIMAL (10, 2)  NOT NULL,
    [StateGstAmount]   DECIMAL (10, 2)  NOT NULL,
    [NetProductAmount] DECIMAL (20, 4)  NOT NULL,
    [CreatedOn]        DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]        UNIQUEIDENTIFIER NULL,
    [ModifiedOn]       DATETIME         NOT NULL DEFAULT (GETDATE()),
    [ModifiedBy]       UNIQUEIDENTIFIER NULL,
    -- 'I' for Insert, 'U' for Update, 'D' for Delete
    [OperationType]    CHAR (1)         NULL,
    CONSTRAINT [FK_InvInvoice_Items_Audit_InvInvoice_Items] FOREIGN KEY ([ItemCode]) REFERENCES [dbo].[InvInvoice_Items] ([ItemCode])
);
