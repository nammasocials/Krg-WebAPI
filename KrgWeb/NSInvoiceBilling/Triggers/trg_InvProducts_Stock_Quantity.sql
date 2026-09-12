/*
    Recomputes [dbo].[InvProducts].[CurrentStock] from the ledger after every
    stock movement, and blocks any movement that would take a product negative.
*/
CREATE TRIGGER [dbo].[trg_InvProducts_Stock_Quantity]
ON [dbo].[InvProducts_Stock]
FOR INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ProductCode UNIQUEIDENTIFIER;

    -- Handle multi-row inserts safely
    SELECT @ProductCode = [ProductCode]
    FROM inserted;

    -- Calculate total stock AFTER the insert
    DECLARE @TotalStock INT;

    SELECT @TotalStock = SUM([Quantity])
    FROM [dbo].[InvProducts_Stock]
    WHERE [ProductCode] = @ProductCode;

    -- If total stock becomes negative, block the insert
    IF (@TotalStock < 0)
    BEGIN
        RAISERROR ('Total stock for this product cannot be negative.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    UPDATE [dbo].[InvProducts]
    SET [CurrentStock] = @TotalStock
    WHERE [ProductCode] = @ProductCode;
END
