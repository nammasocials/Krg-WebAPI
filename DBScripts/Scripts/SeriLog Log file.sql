CREATE DATABASE NSinvoiceBilling;
GO
Use NSinvoiceBilling;
GO

CREATE TABLE Logs (
    Id INT IDENTITY PRIMARY KEY,
    Message NVARCHAR(MAX),
    MessageTemplate NVARCHAR(MAX),
    Level NVARCHAR(128),
    TimeStamp DATETIME,
    Exception NVARCHAR(MAX),
    Properties NVARCHAR(MAX)
);

CREATE TABLE [dbo].[UserType] (
    [UserTypeId] SMALLINT IDENTITY(1,1) PRIMARY KEY,
    [UserTypeName] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE())
);
Go
Insert into UserType (UserTypeName, Description) Values ('SuperAdmin','Super Admin users for maintenance purpose only');
Go
CREATE TABLE [dbo].[InvUser] (
    [UserCode] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL,
    [Password] NVARCHAR(255) NOT NULL,
    [FullName] NVARCHAR(200) NOT NULL,
    [UserType] SMALLINT NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT FK_InvUser_UserType FOREIGN KEY ([UserType])
        REFERENCES [dbo].[UserType]([UserTypeId])
);
Go
Insert into InvUser (Username, Password, FullName, UserType) --PassWord@123$
Values ('NSAdmin','d04aa783775fa330e0ef7b78b329aa02b9ff004d2ae0ab39e2e0caa84a6468bc80c7316d43d62efd1f200a433242eb858e7fb3c0a447906deacf9ef3de3ddc2c','Namma Socials Admin', 1);
Go



--CREATE LOGIN KrgApiUser WITH PASSWORD = 'Krg@10121796';
--USE NSinvoiceBilling;
--CREATE USER KrgApiUser FOR LOGIN KrgApiUser;

---- 3️⃣  Give data access (read/write)
--ALTER ROLE db_datareader ADD MEMBER KrgApiUser;
--ALTER ROLE db_datawriter ADD MEMBER KrgApiUser;
--GRANT VIEW DEFINITION TO [KrgApiUser];