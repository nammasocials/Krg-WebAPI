Use NSinvoiceBilling;
GO

CREATE TABLE [dbo].[InvCustomers] (
    [CustomerCode] Integer IDENTITY(1,1) PRIMARY KEY,
    [CustomerName] NVARCHAR(100) NOT NULL,
    [CustomerAddress] NVARCHAR(500) NOT NULL,
    [GST] NVARCHAR(30) NOT NULL,
    [CustomerLogo] VARBINARY(MAX),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] int
);
Go

/****** Object:  View [dbo].[VCustomers]    Script Date: 24/10/2025 14:00:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VCustomers] AS
    Select 
        [CustomerCode],
        [CustomerName],
        [CustomerAddress],
        [GST],
        [CreatedOn],
        [CreatedBy] 
    from InvCustomers;
GO
