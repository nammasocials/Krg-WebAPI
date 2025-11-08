use NSinvoiceBilling;
go
CREATE PROCEDURE sp_LogActivityFromCustomerAudit
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Insert activity log entries for insert and update operationTypes from InvCustomers_Audit
    INSERT INTO ActivityLog (
        EntityType,
        EntityId,
        ActionType,
        Description,
        CreatedBy,
        CreatedDate,
        RedirectUrl
    )
    SELECT 
        'Customer' AS EntityType,
        CustomerCode AS EntityId,
        CASE 
            WHEN OperationType = 'I' THEN 'Insert'
            WHEN OperationType = 'U' THEN 'Update'
            ELSE NULL
        END AS ActionType,
        CASE 
            WHEN OperationType = 'I' THEN CONCAT('A new customer with name ', CustomerName, ' is added.')
            WHEN OperationType = 'U' THEN 'Modified columns in customer - Name record is modified.'
            ELSE NULL
        END AS Description,
        ModifiedBy AS CreatedBy,
        GETDATE() AS CreatedDate,
        NULL AS RedirectUrl
    FROM InvCustomers_Audit
    WHERE OperationType IN ('I', 'U');
END;

