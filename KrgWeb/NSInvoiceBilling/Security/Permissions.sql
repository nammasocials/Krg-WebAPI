/*
    Explicit grants and denies for [KrgApiUser].

    GRANT / DENY are already idempotent, so these simply re-apply on every
    deployment. Skipped entirely when the user was not created.
*/
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE [name] = N'KrgApiUser')
BEGIN
    PRINT '  -> Applying permissions for [KrgApiUser]';

    -- Stored procedures and functions (InvProducts_StockUpdate, usp_CalculateAndUpdateGST).
    GRANT EXECUTE TO [KrgApiUser];

    -- Serilog's MSSqlServer sink creates its target table on first run.
    GRANT CREATE TABLE TO [KrgApiUser];

    GRANT VIEW DEFINITION TO [KrgApiUser];

    -- Stock rows are append-only; CurrentStock is maintained by
    -- trg_InvProducts_Stock_Quantity, never by the API directly.
    DENY UPDATE ON OBJECT::[dbo].[InvProducts_Stock] TO [KrgApiUser];
END
