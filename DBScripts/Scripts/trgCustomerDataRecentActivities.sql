use NSinvoiceBilling;
go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Anand
-- Create date: 08-11-2025
-- Description:	Recent Activities Trigger
-- =============================================
CREATE TRIGGER trgCustomerDataRecentActivities 
   ON  InvCustomers 
   AFTER INSERT,UPDATE
AS 
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
    BEGIN
       	INSERT INTO [dbo].[ActivityLog]
           ([EntityType]
           ,[EntityId]
           ,[ActionType]
           ,[Description]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[RedirectUrl])
        SELECT 'Customer', CustomerCode, 'INSERT','New Custonmer Data inserted',CreatedBy, GETDATE()
        FROM inserted;
    END
    ELSE IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO [dbo].[ActivityLog]
           ([EntityType]
           ,[EntityId]
           ,[ActionType]
           ,[Description]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[RedirectUrl])
        SELECT 'Customer', CustomerCode, 'UPDATE','Custonmer Data Modified',CreatedBy, GETDATE()
        FROM inserted i;
    END
END
GO
