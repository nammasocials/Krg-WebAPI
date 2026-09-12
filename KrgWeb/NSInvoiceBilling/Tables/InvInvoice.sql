CREATE TABLE [dbo].[InvInvoice] (
    [InvoiceCode]          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [InvoiceNo]            NVARCHAR (100)   NOT NULL UNIQUE,
    [CustomerCode]         UNIQUEIDENTIFIER NOT NULL,
    [isEwayBillAvailable]  BIT              NOT NULL DEFAULT 0,
    [EWayBillLogo]         VARBINARY (MAX)  NULL,
    [EWayBillLogoMime]     NVARCHAR (30)    NULL,
    [GST]                  NVARCHAR (30)    NOT NULL,
    [TotalCost]            DECIMAL (10, 2)  NOT NULL,
    [CreatedOn]            DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]            UNIQUEIDENTIFIER NULL,
    [ModifiedOn]           DATETIME         NOT NULL DEFAULT (GETDATE()),
    [ModifiedBy]           UNIQUEIDENTIFIER NULL,
    CONSTRAINT [FK_InvCustomer] FOREIGN KEY ([CustomerCode]) REFERENCES [dbo].[InvCustomers] ([CustomerCode])
);
