/*
    Records one stock movement for a product. @isAddStock selects the
    Stock-In / Stock-Out constant; [dbo].[trg_InvProducts_Stock_Quantity] then
    recomputes [dbo].[InvProducts].[CurrentStock] from the ledger.
*/
CREATE PROCEDURE [dbo].[InvProducts_StockUpdate]
(
    @ProductCode UNIQUEIDENTIFIER,
    @Quantity    INT,
    @isAddStock  BIT
)
AS
BEGIN
    DECLARE @Key INT;

    IF @isAddStock = 1
        SELECT @Key = [Key]
        FROM [dbo].[InvConstant]
        WHERE [Category] = 'StockTxnType'
          AND [EntityId] = 'InvProducts'
          AND [Name] = 'Stock-In';
    ELSE
        SELECT @Key = [Key]
        FROM [dbo].[InvConstant]
        WHERE [Category] = 'StockTxnType'
          AND [EntityId] = 'InvProducts'
          AND [Name] = 'Stock-Out';

    INSERT INTO [dbo].[InvProducts_Stock]
    (
        [ProductCode],
        [Quantity],
        [TxnType]
    )
    VALUES
    (
        @ProductCode,
        @Quantity,
        @Key
    );
END
