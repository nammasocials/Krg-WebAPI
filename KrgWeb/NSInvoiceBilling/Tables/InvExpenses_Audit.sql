CREATE TABLE [dbo].[InvExpenses_Audit] (
    [AuditID]           INT              IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [ExpenseCode]       UNIQUEIDENTIFIER NOT NULL,
    [ExpenseType]       INT              NOT NULL,
    [ExpenseDate]       DATETIME         NOT NULL DEFAULT (GETDATE()),
    [Description]       NVARCHAR (500)   NULL,
    [Amount]            DECIMAL (18, 2)  NOT NULL,
    [InvoiceCode]       UNIQUEIDENTIFIER NULL,
    [VendorName]        NVARCHAR (200)   NULL,
    [VendorInvoiceNo]   NVARCHAR (100)   NULL,
    [VendorInvoiceDate] DATETIME         NULL,
    [isActive]          BIT              NOT NULL DEFAULT 1,
    [CreatedOn]         DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOn]        DATETIME         NOT NULL DEFAULT (GETDATE()),
    [ModifiedBy]        UNIQUEIDENTIFIER NULL,
    -- 'I' for Insert, 'U' for Update, 'D' for Delete
    [OperationType]     CHAR (1)         NULL,
    CONSTRAINT [FK_InvExpenses_Audit_InvExpenses] FOREIGN KEY ([ExpenseCode]) REFERENCES [dbo].[InvExpenses] ([ExpenseCode])
);
