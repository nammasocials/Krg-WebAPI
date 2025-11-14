--CREATE DATABASE NSinvoiceBilling;
--GO
Use NSinvoiceBilling;
GO

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
    CONSTRAINT FK_InvCustomers_Audit_InvCustomers 
    FOREIGN KEY (CustomerCode) REFERENCES InvCustomers(CustomerCode)
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

CREATE OR ALTER TRIGGER trg_InvCustomers_Audit
ON InvCustomers
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO InvCustomers_Audit (
        [CustomerCode],
        [CustomerName],
        [CustomerEmail],
        [ContactNo],
        [SecnContactNo],
        [CustomerAddress],
        [GST],
        [CustomerLogo],
        [CustomerLogoMime],
        [CreatedOn],
        [CreatedBy],
        [ModifiedOn],
        [ModifiedBy],
        OperationType
    )
    SELECT
        i.[CustomerCode],
        i.[CustomerName],
        i.[CustomerEmail],
        i.[ContactNo],
        i.[SecnContactNo],
        i.[CustomerAddress],
        i.[GST],
        i.[CustomerLogo],
        i.[CustomerLogoMime],
        i.[CreatedOn],
        i.CreatedBy,
        i.[ModifiedOn],
        i.ModifiedBy,
        'I'
    FROM inserted i
    LEFT JOIN deleted d ON i.[CustomerCode] = d.[CustomerCode]
    WHERE d.[CustomerCode] IS NULL; -- Only rows newly inserted

    -- Insert audit for deleted rows (DELETE)
    INSERT INTO InvCustomers_Audit (
        [CustomerCode],
        [CustomerName],
        [CustomerEmail],
        [ContactNo],
        [SecnContactNo],
        [CustomerAddress],
        [GST],
        [CustomerLogo],
        [CustomerLogoMime],
        [CreatedOn],
        [CreatedBy],
        [ModifiedOn],
        [ModifiedBy],
        OperationType
    )
    SELECT
        d.[CustomerCode],
        d.[CustomerName],
        d.[CustomerEmail],
        d.[ContactNo],
        d.[SecnContactNo],
        d.[CustomerAddress],
        d.[GST],
        d.[CustomerLogo],
        d.[CustomerLogoMime],
        d.[CreatedOn],
        d.CreatedBy,
        d.[ModifiedOn],
        d.ModifiedBy,
        'D'
    FROM deleted d
    LEFT JOIN inserted i ON i.[CustomerCode] = d.[CustomerCode]
    WHERE i.[CustomerCode] IS NULL; -- Only rows deleted

    -- Insert audit for updated rows (UPDATE)
    INSERT INTO InvCustomers_Audit (
        [CustomerCode],
        [CustomerName],
        [CustomerEmail],
        [ContactNo],
        [SecnContactNo],
        [CustomerAddress],
        [GST],
        [CustomerLogo],
        [CustomerLogoMime],
        [CreatedOn],
        [CreatedBy],
        [ModifiedOn],
        [ModifiedBy],
        OperationType
    )
    SELECT
        i.[CustomerCode],
        i.[CustomerName],
        i.[CustomerEmail],
        i.[ContactNo],
        i.[SecnContactNo],
        i.[CustomerAddress],
        i.[GST],
        i.[CustomerLogo],
        i.[CustomerLogoMime],
        i.[CreatedOn],
        i.CreatedBy,
        i.[ModifiedOn],
        i.ModifiedBy,
        'U'
    FROM inserted i
    JOIN deleted d ON i.[CustomerCode] = d.[CustomerCode];
END
Go
