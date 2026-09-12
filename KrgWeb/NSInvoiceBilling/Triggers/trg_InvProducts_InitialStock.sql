/*
    Seeds the stock ledger with a product's opening [CurrentStock].

    In 07.InvProducts_Stock.sql @isAddStock is declared but never assigned, so it
    reaches [dbo].[InvProducts_StockUpdate] as NULL and the opening stock is filed
    against the Stock-Out constant. Opening stock is inward, so it is passed as 1
    here instead.

    This changes only the label on the ledger row.
    [dbo].[trg_InvProducts_Stock_Quantity] derives CurrentStock from SUM(Quantity)
    without looking at [TxnType], so no stock figure moves -- but the stock ledger
    report reads direction from the label, and read the opening stock backwards.

    Rows written before this fix still carry the old label; see the note in the
    stock ledger report if historical movements need correcting.
*/
CREATE TRIGGER [dbo].[trg_InvProducts_InitialStock]
ON [dbo].[InvProducts]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ProductCode UNIQUEIDENTIFIER, @Quantity INT;

    SELECT
        @ProductCode = i.[ProductCode],
        @Quantity    = i.[CurrentStock]
    FROM inserted i;

    EXEC [dbo].[InvProducts_StockUpdate] @ProductCode, @Quantity, 1;
END
