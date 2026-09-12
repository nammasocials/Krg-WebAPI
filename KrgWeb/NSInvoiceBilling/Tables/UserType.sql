CREATE TABLE [dbo].[UserType] (
    [UserTypeId]   SMALLINT       IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [UserTypeName] NVARCHAR (255) NOT NULL,
    [Description]  NVARCHAR (500) NOT NULL,
    [CreatedOn]    DATETIME       NOT NULL DEFAULT (GETDATE())
);
