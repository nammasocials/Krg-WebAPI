CREATE TABLE [dbo].[ActivityLog] (
    [ActivityId]  INT              IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [EntityType]  NVARCHAR (50)    NOT NULL,
    [EntityId]    UNIQUEIDENTIFIER NOT NULL,
    [ActionType]  NVARCHAR (20)    NOT NULL,
    [Description] NVARCHAR (500)   NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    [CreatedDate] DATETIME         NULL DEFAULT (GETDATE()),
    [RedirectUrl] NVARCHAR (300)   NULL
);
