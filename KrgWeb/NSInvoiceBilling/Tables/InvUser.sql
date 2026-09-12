CREATE TABLE [dbo].[InvUser] (
    [UserCode]  UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [Username]  NVARCHAR (100)   NOT NULL,
    [Password]  NVARCHAR (255)   NOT NULL,
    [FullName]  NVARCHAR (200)   NOT NULL,
    [UserType]  SMALLINT         NOT NULL,
    [CreatedOn] DATETIME         NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT [FK_InvUser_UserType] FOREIGN KEY ([UserType]) REFERENCES [dbo].[UserType] ([UserTypeId])
);
