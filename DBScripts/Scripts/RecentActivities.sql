CREATE TABLE ActivityLog (
    ActivityId INT IDENTITY(1,1) PRIMARY KEY,
    EntityType NVARCHAR(50) NOT NULL,       
    EntityId INT NOT NULL,                
    ActionType NVARCHAR(20) NOT NULL,      
    Description NVARCHAR(500) NULL, 
    CreatedBy NVARCHAR(100) NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    RedirectUrl NVARCHAR(300) NULL
);
