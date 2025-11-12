Use NSinvoiceBilling;
GO
CREATE TABLE InvCustomers_Audit
(
    AuditID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerCode UNIQUEIDENTIFIER NOT NULL,
    -- Add all other columns from InvCustomers here, e.g.
    [CustomerName] NVARCHAR(100) NOT NULL,
    [CustomerEmail] NVARCHAR(100) NOT NULL,
    [ContactNo] NVARCHAR(15) NOT NULL,
    [SecnContactNo] NVARCHAR(15) NULL,
    [CustomerAddress] NVARCHAR(500) NOT NULL,
    [GST] NVARCHAR(30) NOT NULL,
    [CustomerLogo] VARBINARY(MAX),
	[CustomerLogoMime] Nvarchar(30),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn] DATETIME,
    [ModifiedBy] UNIQUEIDENTIFIER,
    -- ... more columns ...
    OperationType CHAR(1), -- 'I' for Insert, 'U' for Update, 'D' for Delete
);
Go

CREATE TABLE [dbo].[InvCustomers] (
    [CustomerCode] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [CustomerName] NVARCHAR(100) NOT NULL,
    [CustomerEmail] NVARCHAR(100) NOT NULL,
    [ContactNo] NVARCHAR(15) NOT NULL,
    [SecnContactNo] NVARCHAR(15) NULL,
    [CustomerAddress] NVARCHAR(500) NOT NULL,
    [GST] NVARCHAR(30) NOT NULL,
    [CustomerLogo] VARBINARY(MAX),
	[CustomerLogoMime] Nvarchar(30),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn] DATETIME,
    [ModifiedBy] UNIQUEIDENTIFIER
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
        [CustomerEmail],
        [ContactNo],
        [SecnContactNo],
        [GST],
        [CreatedOn],
        [CreatedBy],
        [ModifiedOn],
        [ModifiedBy] 
    from InvCustomers;
GO
