CREATE OR ALTER TRIGGER trg_AfterInsert_InvInvoiceItems
ON InvInvoice_Items
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Invoice may have multiple rows inserted at once
    DECLARE @InvoiceCodes TABLE (InvoiceCode UNIQUEIDENTIFIER);

    INSERT INTO @InvoiceCodes (InvoiceCode)
    SELECT DISTINCT InvoiceCode FROM inserted;

    DECLARE @InvCode UNIQUEIDENTIFIER;

    DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
        SELECT InvoiceCode FROM @InvoiceCodes;

    OPEN cur;
    FETCH NEXT FROM cur INTO @InvCode;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC usp_CalculateAndUpdateGST @InvCode;
        FETCH NEXT FROM cur INTO @InvCode;
    END

    CLOSE cur;
    DEALLOCATE cur;
END