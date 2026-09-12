CREATE TABLE [dbo].[InvCustomers] (
    [CustomerCode]      UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
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
    [ModifiedBy]        UNIQUEIDENTIFIER NULL
);
