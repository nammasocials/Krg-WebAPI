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
Insert into InvUser (Username, Password, FullName, UserType) --Password123$
Values ('NSAdmin','9183554fe5425ffe6aadfa6315792352119df073f92606a177458af117e2f0efe44c6fcefedfcb1231b1f7437ef272bd3f09cf0425bd4ef12a231e265c974992','Namma Socials Admin', 1);
Go
