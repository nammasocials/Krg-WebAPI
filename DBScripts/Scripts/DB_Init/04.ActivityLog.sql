Use NSinvoiceBilling;
GO
CREATE TABLE ActivityLog (
    ActivityId INT IDENTITY(1,1) PRIMARY KEY,
    EntityType NVARCHAR(50) NOT NULL,       
    EntityId UNIQUEIDENTIFIER NOT NULL,                
    ActionType NVARCHAR(20) NOT NULL,      
    Description NVARCHAR(500) NULL, 
    CreatedBy UNIQUEIDENTIFIER,
    CreatedDate DATETIME DEFAULT GETDATE(),
    RedirectUrl NVARCHAR(300) NULL
);

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VActivityLog] AS
    Select 
    ActivityId,
    EntityType, 
    EntityId, 
    ActionType, 
    Description, 
    CreatedBy, 
    CreatedDate, 
    RedirectUrl 
    from ActivityLog;
GO