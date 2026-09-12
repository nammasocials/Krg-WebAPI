/*
    Expenses, both kinds, in one table.

      * A normal expense (electricity, cleaning) fills in only the type, amount,
        date and description.
      * An expense attributable to a sale sets [InvoiceCode].
      * An expense that came in on someone else's bill sets the [Vendor*]
        columns. Those invoices do not exist in this database, so they are free
        text rather than a foreign key.

    Both links are independently nullable and may appear together: a freight
    vendor's bill raised against one of our invoices sets both.

    [ExpenseType] is an [dbo].[InvConstant] key
    (EntityId = 'InvExpenses', Category = 'ExpenseType').
*/
CREATE TABLE [dbo].[InvExpenses] (
    [ExpenseCode]       UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [ExpenseType]       INT              NOT NULL,
    [ExpenseDate]       DATETIME         NOT NULL DEFAULT (GETDATE()),
    [Description]       NVARCHAR (500)   NULL,
    [Amount]            DECIMAL (18, 2)  NOT NULL,
    -- Link to one of our own invoices. NULL for a normal expense.
    [InvoiceCode]       UNIQUEIDENTIFIER NULL,
    -- Supplier bill this expense arrived on. NULL when there is no vendor bill.
    [VendorName]        NVARCHAR (200)   NULL,
    [VendorInvoiceNo]   NVARCHAR (100)   NULL,
    [VendorInvoiceDate] DATETIME         NULL,
    [isActive]          BIT              NOT NULL DEFAULT 1,
    [CreatedOn]         DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [ModifiedOn]        DATETIME         NOT NULL DEFAULT (GETDATE()),
    [ModifiedBy]        UNIQUEIDENTIFIER NULL,
    CONSTRAINT [FK_InvExpenses_InvInvoice] FOREIGN KEY ([InvoiceCode]) REFERENCES [dbo].[InvInvoice] ([InvoiceCode]),
    CONSTRAINT [Chk_InvExpenses_Amount_positive] CHECK ([Amount] > 0)
);
GO

CREATE NONCLUSTERED INDEX [IX_InvExpenses_ExpenseDate]
    ON [dbo].[InvExpenses] ([ExpenseDate] DESC) INCLUDE ([ExpenseType], [Amount]);
GO

-- Deliberately not filtered on "[InvoiceCode] IS NOT NULL": a filtered index makes
-- every INSERT require SET QUOTED_IDENTIFIER ON, which sqlcmd does not set by
-- default, so seeding and maintenance scripts would fail for no real gain here.
CREATE NONCLUSTERED INDEX [IX_InvExpenses_InvoiceCode]
    ON [dbo].[InvExpenses] ([InvoiceCode]);
