--CREATE DATABASE NSinvoiceBilling;
--GO
Use NSinvoiceBilling;
GO
CREATE TABLE [dbo].[UserType] (
    [UserTypeId] SMALLINT IDENTITY(1,1) PRIMARY KEY,
    [UserTypeName] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE())
);
Go
Insert into UserType (UserTypeName, Description) Values ('SuperAdmin','Super Admin users for maintenance purpose only');
Go
Insert into UserType (UserTypeName, Description) Values ('Admin','Admin users for making Entry purpose only');
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
Insert into InvUser (Username, Password, FullName, UserType) --AnandSiva@123$
Values ('Anand','ce5fcfa41b61a411c438dee277898af452f2973e28352df79a0f80f0ae42aa9c0caf0ee85a6e2d63f91f7f329cb9ba98cddb27629137b47f8fe917b506cfb8f8','Krg Admin', 2);
Go
--truncate table InvUser


--CREATE LOGIN KrgApiUser WITH PASSWORD = 'Krg@10121796';
--USE NSinvoiceBilling;
--CREATE USER KrgApiUser FOR LOGIN KrgApiUser;

---- 3??  Give data access (read/write)
--ALTER ROLE db_datareader ADD MEMBER KrgApiUser;
--ALTER ROLE db_datawriter ADD MEMBER KrgApiUser;
--GRANT VIEW DEFINITION TO [KrgApiUser];